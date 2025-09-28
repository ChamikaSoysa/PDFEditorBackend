using Microsoft.AspNetCore.Mvc;
using PDFBackend.Interfaces;

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

        public IActionResult Index()
        {
            return View();
        }
    }
}
