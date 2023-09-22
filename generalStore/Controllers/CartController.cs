using AspNetCoreHero.ToastNotification.Abstractions;
using generalStore.Data;
using generalStore.Infrastructure;
using generalStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace generalStore.Controllers
{
    public class CartController : Controller
    {
        public Cart? Cart { get; set; }


        private readonly ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;

        public CartController(ApplicationDbContext context, INotyfService toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
            return View("Cart", HttpContext.Session.GetJson<Cart>("cart"));
        }

        public IActionResult Checkout()
        {
            return View("Checkout", HttpContext.Session.GetJson<Cart>("cart"));
        }





        public IActionResult AddToCart(int productId)
        {
            Product? product = _context.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, 1);
                HttpContext.Session.SetJson<Cart>("cart", Cart);
            }
            return View("Cart", Cart);
        }

        public IActionResult ReduceToCart(int productId)
        {
            Product? product = _context.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, -1);
                HttpContext.Session.SetJson<Cart>("cart", Cart);
            }
            return View("Cart", Cart);
        }

        public IActionResult RemoveFromCart(int productId)
        {
            Product? product = _context.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart");
                Cart.RemoveLine(product);
                HttpContext.Session.SetJson<Cart>("cart", Cart);
            }
            return View("Cart", Cart);
        }

        

    }
}
