using Microsoft.AspNetCore.Mvc;
using RunGroops.Data;
using RunGroops.Interfaces;
using RunGroops.Models;
using RunGroops.ViewModels;

namespace RunGroops.Controllers {
    public class RaceController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;

        public RaceController(ApplicationDbContext context, IRaceRepository raceRepository, IPhotoService photoService) {
            this._context = context;
            this._raceRepository = raceRepository;
            this._photoService = photoService;
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
        public async Task<IActionResult> Create(CreateRaceViewModel raceViewModel) {
            if (ModelState.IsValid) {
                var result = await _photoService.AddPhotoAsync(raceViewModel.Image);
                var race = new Race {
                    Title = raceViewModel.Title,
                    Address = new Address {
                        Street = raceViewModel.Address.Street,
                        City = raceViewModel.Address.City,
                        State = raceViewModel.Address.State
                    },
                    Description = raceViewModel.Description,
                    RaceCategory = raceViewModel.RaceCategory,
                    Image = result.Url.ToString()
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else {
                ModelState.AddModelError("", "Can't upload image");
            }
            return View(raceViewModel);
        }
    }


}
