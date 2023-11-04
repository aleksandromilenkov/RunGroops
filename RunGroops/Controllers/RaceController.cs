using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroops.Data;
using RunGroops.Models;

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

        public IActionResult Detail(int id) {
            Race race = _context.Races.Include(a => a.Address).FirstOrDefault(x => x.Id == id);
            return View(race);
        }
    }
}
