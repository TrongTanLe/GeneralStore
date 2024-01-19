using generalStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace generalStore.Components
{
    public class Trandy : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public Trandy(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(_context.Products.Where(x => x.IsTrandy==true)
                .OrderByDescending(x => x.DateCreated)
                .Take(8)
                .ToList());
        }
    }
}
