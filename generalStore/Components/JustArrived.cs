using generalStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace generalStore.Components
{
    public class JustArrived : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public JustArrived(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(_context.Products.Where(x => x.IsArrived == true)
                .OrderByDescending(x => x.DateCreated)
                .Take(8)
                .ToList());
        }
    }
}
