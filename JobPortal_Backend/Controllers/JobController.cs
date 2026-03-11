using Microsoft.AspNetCore.Mvc;

namespace JobPortal_Backend.Controllers
{
    public class JobController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
