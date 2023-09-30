using generalStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace generalStore.Controllers
{
    public class LocationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationController(ApplicationDbContext context) 
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

/*        public ActionResult QuanHuyenList(int LocationId)
        {
            var QuanHuyens = _context.Locations.OrderBy(x=> x.LocationId)
                .Where(x => x.Parent == LocationId && x.Levels == 2)
                .OrderBy(x => x.Name)
                .ToList();
            return Json(QuanHuyens);
        }

        public ActionResult PhuongXaList(int LocationId)
        {
            var QuanXas = _context.Locations.OrderBy(x => x.LocationId)
                .Where(x => x.Parent == LocationId && x.Levels == 3)
                .OrderBy(x => x.Name)
                .ToList();
            return Json(QuanXas);
        }*/

    }
}
