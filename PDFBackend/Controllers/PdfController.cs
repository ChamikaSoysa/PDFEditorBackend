using Microsoft.AspNetCore.Mvc;
using PDFBackend.DTOs;
using PDFBackend.Interfaces;
using System.Text.Json;

namespace PDFBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : Controller
    {

        private readonly IFileStorageService _fileStorage;
        private readonly IPdfService _pdfService;
        private readonly ILogger<PdfController> _logger;

        public PdfController(IFileStorageService fileStorage, IPdfService pdfService, ILogger<PdfController> logger)
        {
            _fileStorage = fileStorage;
            _pdfService = pdfService;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var path = await _fileStorage.SaveFileAsync(file);
            return Ok(new { filePath = path });
        }

        [HttpPost("edit-text")]
        public async Task<IActionResult> EditText([FromForm] string filePath, [FromForm] string editsJson)
        {
            var edits = JsonSerializer.Deserialize<Dictionary<int, List<TextEditDto>>>(editsJson)!;
            var result = await _pdfService.UpdatePdfTextAsync(filePath, edits);
            await _fileStorage.DeleteFileAsync(filePath);
            return File(result, "application/pdf", "edited.pdf");
        }

        [HttpPost("edit-metadata")]
        public async Task<IActionResult> EditMetadata([FromForm] string filePath, [FromForm] MetadataDto metadata)
        {
            var result = await _pdfService.UpdatePdfMetadataAsync(filePath, metadata);
            await _fileStorage.DeleteFileAsync(filePath);
            return File(result, "application/pdf", "edited.pdf");
        }

        [HttpGet("export/{format}")]
        public async Task<IActionResult> Export(string format, [FromQuery] string filePath)
        {
            byte[] result;
            string contentType, fileName;

            switch (format.ToLowerInvariant())
            {
                case "pdf":
                    result = await _fileStorage.ReadFileAsync(filePath);
                    contentType = "application/pdf";
                    fileName = "document.pdf";
                    break;
                case "docx":
                    result = await _pdfService.ConvertToDocxAsync(filePath);
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    fileName = "document.docx";
                    break;
                case "images":
                    result = await _pdfService.ConvertToImagesZipAsync(filePath);
                    contentType = "application/zip";
                    fileName = "pages.zip";
                    break;
                default:
                    return BadRequest("Invalid format. Use: pdf, docx, images");
            }

            await _fileStorage.DeleteFileAsync(filePath);
            return File(result, contentType, fileName);
        }

    }
}
