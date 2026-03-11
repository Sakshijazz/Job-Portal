using Microsoft.AspNetCore.Mvc;

namespace JobPortal_Backend.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
