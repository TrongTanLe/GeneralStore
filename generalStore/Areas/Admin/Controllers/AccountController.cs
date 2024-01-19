using Microsoft.AspNetCore.Mvc;

namespace generalStore.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
