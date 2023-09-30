using AspNetCoreHero.ToastNotification.Abstractions;
using generalStore.Data;
using generalStore.Infrastructure;
using generalStore.Models;
using generalStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace generalStore.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;

        public CheckoutController(ApplicationDbContext context, INotyfService toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
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

        [Route("checkout", Name = "Checkout")]
        public IActionResult Index(string returnUrl = null)
        {
            //Lay gio hang ra de xu ly
            var cart = HttpContext.Session.GetJson<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            MuaHangVM model = new MuaHangVM();
            if(taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x=>x.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.FullName;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Addres;
            }
            ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "Locations", "Name");
            ViewBag.GioHang = cart;
            return View(model);
        }

        [HttpPost]
        [Route("checkout", Name = "Checkout")]
        public IActionResult Index(MuaHangVM muaHang)
        {
            //Lay gio hang ra de xu ly
            var cart = HttpContext.Session.GetJson<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            MuaHangVM model = new MuaHangVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.FullName;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Addres;

                khachhang.LocationId = muaHang.TinhThanh;
                khachhang.District = muaHang.QuanHuyen;
                khachhang.Ward = muaHang.PhuongXa;
                khachhang.Addres = muaHang.Address;

                _context.Update(khachhang);
                _context.SaveChanges();
            }
           /* try
            {
                if (ModelState.IsValid)
                {
                    Order donhang = new Order();
                    donhang.CustomerId = model.CustomerId;
                    donhang. = model.Address;
                    donhang. = model.TinhThanh;
                    donhang.CustomerId = model.QuanHuyen;
                    donhang. = model.PhuongXa;
                }
            }*/


            ViewData["lsTinhThanh"] = new SelectList(_context.Locations.Where(x => x.Levels == 1).OrderBy(x => x.Type).ToList(), "Locations", "Name");
            ViewBag.GioHang = cart;
            return View(model);
        }
    }
}
