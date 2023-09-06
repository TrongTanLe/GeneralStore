using generalStore.Infrastructure;
using generalStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace generalStore.Components
{
    public class CartWidget : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(HttpContext.Session.GetJson<Cart>("cart"));
        }
    }
}
