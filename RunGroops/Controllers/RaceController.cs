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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceController(ApplicationDbContext context, IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor) {
            this._context = context;
            this._raceRepository = raceRepository;
            this._photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
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
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createClubViewModel = new CreateRaceViewModel { AppUserId = curUserId };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceViewModel) {
            if (ModelState.IsValid) {
                var result = await _photoService.AddPhotoAsync(raceViewModel.Image);
                var race = new Race {
                    Title = raceViewModel.Title,
                    AppUserId = raceViewModel.AppUserId,
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

        public async Task<IActionResult> Edit(int id) {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) {
                return View("Error");
            }
            var raceViewModel = new EditRaceViewModel() {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = new Address {
                    Street = race.Address.Street,
                    City = race.Address.City,
                    State = race.Address.State
                },
                RaceCategory = race.RaceCategory,
                URL = race.Image
            };
            return View(raceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel editRaceViewModel) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Failed to edit race");
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return View("Edit", editRaceViewModel);
            }
            var userRace = await _raceRepository.GetByIdAsyncNoTracking(id);
            if (userRace != null) {
                if (userRace.Image != null) {
                    try {
                        await _photoService.DeletePhotoAsync(userRace.Image);
                    }
                    catch (Exception ex) {
                        ModelState.AddModelError("", "Could not delete photo");
                        return View("Edit", editRaceViewModel);
                    }
                }
                var photoResult = await _photoService.AddPhotoAsync(editRaceViewModel.Image);
                var race = new Race {
                    Id = id,
                    Title = editRaceViewModel.Title,
                    Description = editRaceViewModel.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = editRaceViewModel.AddressId,
                    Address = new Address {
                        Street = editRaceViewModel.Address.Street,
                        City = editRaceViewModel.Address.City,
                        State = editRaceViewModel.Address.State
                    },
                    RaceCategory = editRaceViewModel.RaceCategory,
                };
                _raceRepository.Update(race);
                return RedirectToAction("Index");
            }
            else {
                return View(editRaceViewModel);
            }
        }

        public async Task<IActionResult> Delete(int id) {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) {
                return View("Error");
            }
            return View(race);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id) {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) {
                return View("Error");
            }
            _raceRepository.Delete(race);
            return RedirectToAction("Index");
        }
    }


}
