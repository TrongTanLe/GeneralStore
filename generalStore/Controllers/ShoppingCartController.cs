using AspNetCoreHero.ToastNotification.Abstractions;
using generalStore.Data;
using generalStore.Infrastructure;
using generalStore.Models;
using generalStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace generalStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;

        public ShoppingCartController(ApplicationDbContext context, INotyfService toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        [Route("carts", Name = "Cart")]
        public IActionResult Index()
        {
            var lsGioHang = GioHang;
            return View(GioHang);
        }

        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.GetJson<List<CartItem>>("GioHang");
                if (gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }

        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult AddToCart(int productID, int? amount)
        {
            List<CartItem> gioHang = GioHang;

            try
            {
                //Them san pham vap gio hang
                CartItem item = GioHang.SingleOrDefault(p => p.Product.ProductId == productID);
                if (item != null) //da co => cap nhap so luong san pham
                {
                    if (amount.HasValue)
                    {
                        item.amount = amount.Value;
                    }
                    else
                    {
                        item.amount++;
                    }
                }
                else
                {
                    Product hh = _context.Products.SingleOrDefault(p => p.ProductId == productID);
                    item = new CartItem()
                    {
                        amount = amount.HasValue ? amount.Value : 1,
                        Product = hh
                    };
                    gioHang.Add(item);
                }
                HttpContext.Session.SetJson<List<CartItem>>("GioHang", gioHang);
                _toastNotification.Success("Add product success");
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/remove")]
        public IActionResult Remove(int productID)
        {
            try
            {
                List<CartItem> gioHang = GioHang;
                CartItem item = GioHang.SingleOrDefault(p => p.Product.ProductId == productID);
                if (item != null)
                {
                    gioHang.Remove(item);
                }
                HttpContext.Session.SetJson<List<CartItem>>("GioHang", gioHang);
                _toastNotification.Success("Remove product success");
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart(int productID, int? amount)
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("GioHang");
            try
            {
                if(cart != null)
                {
                    CartItem item = GioHang.SingleOrDefault(p => p.Product.ProductId == productID);
                    if (item != null && amount.HasValue)
                    {
                        item.amount = amount.Value;
                    }
                    HttpContext.Session.SetJson<List<CartItem>>("GioHang", cart);
                }
                
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}
