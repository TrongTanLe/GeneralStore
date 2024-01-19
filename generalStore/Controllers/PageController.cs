using generalStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace generalStore.Controllers
{
    public class PageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PageController(ApplicationDbContext context) 
        { 
            _context = context;
        }

        //GET: page/Alias
        [Route("/page/{Alias}", Name = "PageDetails")]
        public IActionResult Details(string Alias)
        {
            if(string.IsNullOrEmpty(Alias)) return RedirectToAction("Index", "Home");
            var page = _context.Pages.AsNoTracking().SingleOrDefault(x => x.Alias == Alias);
            if (page == null) return RedirectToAction("Index", "Home");
            return View(page);
        }
    }
}
