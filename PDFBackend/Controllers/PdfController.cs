using Microsoft.AspNetCore.Mvc;
using PDFBackend.DTOs;
using PDFBackend.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    private readonly IFileStorageService _fileStorage;
    private readonly IPdfService _pdfService;

    public PdfController(IFileStorageService fileStorage, IPdfService pdfService)
    {
        _fileStorage = fileStorage;
        _pdfService = pdfService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var path = await _fileStorage.SaveFileAsync(file);
        // Return file path AND a download URL for preview
        return Ok(new { filePath = path });
    }

    [HttpPost("preview")]
    public async Task<IActionResult> Preview([FromBody] FilePathRequest request)
    {
        var bytes = await _fileStorage.ReadFileAsync(request.FilePath);
        return File(bytes, "application/pdf");
    }

    [HttpPost("edit-text")]
    public async Task<IActionResult> EditText([FromBody] EditTextRequest request) // ← Changed to JSON
    {
        var result = await _pdfService.UpdatePdfTextAsync(request.FilePath, request.Edits);
        //await _fileStorage.DeleteFileAsync(request.FilePath);
        return File(result, "application/pdf", "edited.pdf");
    }

    [HttpPost("edit-metadata")]
    public async Task<IActionResult> EditMetadata([FromBody] EditMetadataRequest request) // ← JSON
    {
        var result = await _pdfService.UpdatePdfMetadataAsync(request.FilePath, request.Metadata);
        //await _fileStorage.DeleteFileAsync(request.FilePath);
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
                return BadRequest("Invalid format");
        }

        //await _fileStorage.DeleteFileAsync(filePath);
        return File(result, contentType, fileName);
    }
}