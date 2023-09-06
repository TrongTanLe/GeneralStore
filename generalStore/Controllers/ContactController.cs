using Microsoft.AspNetCore.Mvc;

namespace generalStore.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View("Contact");
        }
    }
}
