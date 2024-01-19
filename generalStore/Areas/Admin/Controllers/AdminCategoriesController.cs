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
    public class AdminCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;

        public AdminCategoriesController(ApplicationDbContext context, INotyfService toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        // GET: Admin/AdminCategories
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var lsCategories = _context.Categories
                .AsNoTracking()
                .OrderByDescending(x => x.CategoryId);

            PagedList<Category> models = new PagedList<Category>(lsCategories, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }
            // GET: Admin/AdminCategories/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/AdminCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,CategoryPhoto,CategoryOrder,ParentId,Levels,Title,Alias,MetaDesc,MetaKey,Published,Cover,SchemaMarkup")] Category category, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                category.CategoryName = Utilities.ToTitleCase(category.CategoryName);
                if (fThumb != null) //fthumb = fproductPhoto
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(category.CategoryPhoto) + extension;
                    category.CategoryPhoto = await Utilities.UploadFile(fThumb, @"categories", image.ToLower());
                }
                if (string.IsNullOrEmpty(category.CategoryPhoto)) category.CategoryPhoto = "default.jpg";
                category.Alias = Utilities.SEOUrl(category.CategoryName);             
                _context.Add(category);
                await _context.SaveChangesAsync();
                _toastNotification.Success("Create success");
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/AdminCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,CategoryPhoto,CategoryOrder,ParentId,Levels,Title,Alias,MetaDesc,MetaKey,Published,Cover,SchemaMarkup")] Category category, Microsoft.AspNetCore.Http.IFormFile? fThumb)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (fThumb != null) //fthumb = fproductPhoto
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(category.CategoryPhoto) + extension;
                        category.CategoryPhoto = await Utilities.UploadFile(fThumb, @"categories", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(category.CategoryPhoto)) category.CategoryPhoto = "default.jpg";
                    category.Alias = Utilities.SEOUrl(category.CategoryName);

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    _toastNotification.Success("Edit success");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }

        // GET: Admin/AdminCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
