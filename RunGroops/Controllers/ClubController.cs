using Microsoft.AspNetCore.Mvc;
using RunGroops.Data;
using RunGroops.Models;

namespace RunGroops.Controllers {
    public class ClubController : Controller {
        private readonly ApplicationDbContext _context;
        public ClubController(ApplicationDbContext context) {
            this._context = context;
        }
        public IActionResult Index() {
            var clubs = _context.Clubs.ToList();
            return View(clubs);
        }

        public IActionResult Detail(int id) {
            Club club = _context.Clubs.FirstOrDefault(c => c.Id == id);
            return View(club);
        }
    }
}
