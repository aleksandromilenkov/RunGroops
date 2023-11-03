using Microsoft.AspNetCore.Mvc;
using RunGroops.Data;

namespace RunGroops.Controllers {
    public class RaceController : Controller {
        private readonly ApplicationDbContext _context;
        public RaceController(ApplicationDbContext context) {
            this._context = context;
        }
        public IActionResult Index() {
            var races = _context.Races.ToList();
            return View(races);
        }
    }
}
