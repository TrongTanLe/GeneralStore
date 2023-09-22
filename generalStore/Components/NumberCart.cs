using generalStore.Infrastructure;
using generalStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace generalStore.Components
{
    public class NumberCart : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("GioHang");
            return View(cart);
        }
    }
}
