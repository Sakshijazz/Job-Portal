using Microsoft.AspNetCore.Mvc;

namespace JobPortal_Backend.Controllers
{
    public class ApplicationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
