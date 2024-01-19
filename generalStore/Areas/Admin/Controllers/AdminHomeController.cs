using Microsoft.AspNetCore.Mvc;

namespace generalStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
