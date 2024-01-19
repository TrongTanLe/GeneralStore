using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using generalStore.Data;
using generalStore.Models;
using generalStore.Helpper;
using PagedList.Core;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace generalStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;

        public AdminProductsController(ApplicationDbContext context, INotyfService toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        // GET: Admin/AdminProducts
        public IActionResult Index(int page = 1, int CatID = 0)
        {
            var pageNumber = page; /*== null || page <= 0 ? 1 : page.Value*/
            var pageSize = Utilities.PAGE_SIZE;

            List<Product> lsProducts = new List<Product>();

            if (CatID != 0)
            {
                lsProducts = _context.Products
                .AsNoTracking()
                .Where(x => x.CategoryId == CatID)
                .Include(x => x.Category)
                .Include(x => x.Color)
                .Include(x => x.Size)
                .OrderByDescending(x => x.ProductId).ToList();
            }
            else
            {

                lsProducts = _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Color)
                .Include(x => x.Size)
                .OrderByDescending(x => x.ProductId).ToList();
            }

            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentCateID = CatID;
            ViewBag.CurrentPage = pageNumber;

            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", CatID);

            return View(models);
        }

        public IActionResult Filtter(int CatID = 0)
        {

            var url = $"/Admin/AdminProducts?CatID={CatID}";
            if (CatID == 0)
            {
                url = $"/Admin/AdminProducts";
            }

            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
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
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName");
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,CategoryId,ProductPrice,ProductDiscount,ProductPhoto,SizeId,ColorId,IsTrandy,IsArrived,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Title,Alias,MetaDesc,MetaKey,UnitsInStock")] Product product, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                product.ProductName = Utilities.ToTitleCase(product.ProductName);
                if (fThumb != null) //fthumb = fproductPhoto
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(product.ProductName) + extension;
                    product.ProductPhoto = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                }
                if (string.IsNullOrEmpty(product.ProductPhoto)) product.ProductPhoto = "default.jpg";
                product.Alias = Utilities.SEOUrl(product.ProductName);
                product.DateModified = DateTime.Now;
                product.DateCreated = DateTime.Now;

                _context.Add(product);
                await _context.SaveChangesAsync();
                _toastNotification.Success("Create success");
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorId", product.ColorId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeId", product.SizeId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", product.ColorId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName", product.SizeId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,CategoryId,ProductPrice,ProductDiscount,ProductPhoto,SizeId,ColorId,IsTrandy,IsArrived,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Title,Alias,MetaDesc,MetaKey,UnitsInStock")] Product product, Microsoft.AspNetCore.Http.IFormFile? fThumb)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    product.ProductName = Utilities.ToTitleCase(product.ProductName);
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(product.ProductName) + extension;
                        product.ProductPhoto = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(product.ProductPhoto)) product.ProductPhoto = "default.jpg";
                    product.Alias = Utilities.SEOUrl(product.ProductName);
                    product.DateModified = DateTime.Now;

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    _toastNotification.Success("Edit success");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorName", product.ColorId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "SizeId", "SizeName", product.SizeId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            _toastNotification.Success("Delete success");
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
