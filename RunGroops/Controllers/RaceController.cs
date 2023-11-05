using Microsoft.AspNetCore.Mvc;
using RunGroops.Data;
using RunGroops.Interfaces;
using RunGroops.Models;

namespace RunGroops.Controllers {
    public class RaceController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly IRaceRepository _raceRepository;

        public RaceController(ApplicationDbContext context, IRaceRepository raceRepository) {
            this._context = context;
            this._raceRepository = raceRepository;
        }
        public async Task<IActionResult> Index() {
            var races = await _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id) {
            Race race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Race race) {
            if (!ModelState.IsValid) {
                return View(race);
            }
            _raceRepository.Add(race);
            return RedirectToAction("Index");
        }
    }


}
