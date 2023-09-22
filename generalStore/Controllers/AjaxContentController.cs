using Microsoft.AspNetCore.Mvc;

namespace generalStore.Controllers
{
    public class AjaxContentController : Controller
    {
        public IActionResult HeaderFavourites()
        {
            return ViewComponent("NumberCart");
        }
    }
}
