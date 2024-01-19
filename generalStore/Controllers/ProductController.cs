using generalStore.Data;
using generalStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using static generalStore.Controllers.ProductsController;

namespace generalStore.Controllers
{
    /*[Route("/shop")]*/
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult GetFilteredProducts([FromBody] FilterData filter)
        {
            var filterdProducts = _context.Products.ToList();
            if (filter.Prices != null && filter.Prices.Count > 0 && !filter.Prices.Contains("all"))
            {
                List<Price> prices = new List<Price>();
                foreach (var range in filter.Prices)
                {
                    var value = range.Split("-").ToArray();
                    Price price = new Price();
                    price.Min = Int16.Parse(value[0]);
                    price.Max = Int16.Parse(value[1]);
                    prices.Add(price);
                }
                filterdProducts = filterdProducts.Where(p => prices.Any(r => p.ProductPrice >= r.Min && p.ProductPrice <= r.Max)).ToList();
            };

            if (filter.Colors != null && filter.Colors.Count > 0 && !filter.Colors.Contains("all"))
            {
                filterdProducts = filterdProducts.Where(p => filter.Colors.Contains(p.Color.ColorName)).ToList();
            };

            if (filter.Sizes != null && filter.Sizes.Count > 0 && !filter.Sizes.Contains("all"))
            {
                filterdProducts = filterdProducts.Where(p => filter.Sizes.Contains(p.Size.SizeName)).ToList();
            };
            return PartialView("_ReturnProducts", filterdProducts);
        }

        [Route("/shop", Name = "ShopProduct")]
        public IActionResult Index(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 9;
                var lsProducts = _context.Products
                    .AsNoTracking()
                    .OrderByDescending(x => x.DateCreated);
                PagedList<Product> models = new PagedList<Product>(lsProducts, pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }


        [Route("/{Alias}", Name = "ListProduct")]
        public IActionResult List(string Alias, int page = 1)
        {
            try
            {
                var pageSize = 9;
                var danhmuc = _context.Categories.AsNoTracking().SingleOrDefault(x=>x.Alias == Alias);
                var lsProducts = _context.Products
                    .AsNoTracking()
                    .Where(x => x.CategoryId == danhmuc.CategoryId)
                    .OrderByDescending(x => x.DateCreated);
                PagedList<Product> models = new PagedList<Product>(lsProducts, page, pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.CurrentCat = danhmuc;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [Route("/{Alias}-{id}", Name = "ProductsDetail")]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null || _context.Products == null)
                {
                    return NotFound();
                }

                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Color)
                    .Include(p => p.Size)
                    .FirstOrDefaultAsync(m => m.ProductId == id);
                if (product == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var lsProduct = _context.Products
                    .Where(x=>x.CategoryId == product.CategoryId && x.ProductId != id && x.Active == true)
                    .OrderByDescending(x=>x.DateCreated)
                    .Take(5)
                    .ToList();
                ViewBag.SanPham = lsProduct;
                return View(product);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
    }
}
