using Microsoft.AspNetCore.Mvc;

namespace generalStore.Controllers
{
    public class ContactController : Controller
    {
        [Route("/contact", Name = "Contact")]
        public IActionResult Index()
        {
            return View("Contact");
        }
    }
}
