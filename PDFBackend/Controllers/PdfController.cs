using Microsoft.AspNetCore.Mvc;

namespace PDFBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
