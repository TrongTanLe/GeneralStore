using AspNetCoreHero.ToastNotification.Abstractions;
using generalStore.Data;
using generalStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace generalStore.Controllers
{
    public class SearchProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;

        public SearchProductController(ApplicationDbContext context, INotyfService toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public IActionResult FindProduct(string keyword)
        {
            List<Product> ls = new List<Product>();
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            ls = _context.Products
                .AsNoTracking()
                .Include(a => a.Category)
                .Include(a => a.Color)
                .Include(a => a.Size)
                .Where(x => x.ProductName.Contains(keyword))
                .OrderByDescending(x => x.ProductName)
                .Take(10)
                .ToList();
            if (ls == null)
            {
                return PartialView("ListProductsSearchPartial", null);
            }
            else
            {
                return PartialView("ListProductsSearchPartial", ls);
            }
        }
    }
}
