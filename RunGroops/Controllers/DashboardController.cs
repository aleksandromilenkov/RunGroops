using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroops.Data;
using RunGroops.Interfaces;
using RunGroops.Models;
using RunGroops.ViewModels;

namespace RunGroops.Controllers {
    public class DashboardController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(ApplicationDbContext context, IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService) {
            _context = context;
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }
        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editUserVM, ImageUploadResult photoResult) {
            user.Id = editUserVM.Id;
            user.Pace = editUserVM.Pace;
            user.UserName = editUserVM.Username;
            user.Mileage = editUserVM.Mileage;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.City = editUserVM.City;
            user.State = editUserVM.State;
        }
        public async Task<IActionResult> Index() {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel() { Races = userRaces, Clubs = userClubs };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile() {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            AppUser user = await _dashboardRepository.GetUserById(currentUserId);
            if (user == null) {
                return View("Error");
            }
            var editUserViewModel = new EditUserDashboardViewModel() {
                Id = currentUserId,
                Pace = user.Pace,
                Username = user.UserName,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editUserDashboardViewModel) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Failed to edit the user's profile");
                return View("EditUserProfile", editUserDashboardViewModel);
            }
            AppUser user = await _dashboardRepository.GetUserByIdNoTracking(editUserDashboardViewModel.Id);
            if (user.ProfileImageUrl == "" || user.ProfileImageUrl == null) {
                var photoResult = await _photoService.AddPhotoAsync(editUserDashboardViewModel.Image);
                // Here we have another AppUser variable, so we must select the user from the Db with NoTracking method.
                // And we map the props from the view that user filled to the user in the db and then update it
                MapUserEdit(user, editUserDashboardViewModel, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else {
                try {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception e) {
                    ModelState.AddModelError("", "Could not delete the photo");
                    return View(editUserDashboardViewModel);
                }
                var photoResult = await _photoService.AddPhotoAsync(editUserDashboardViewModel.Image);
                MapUserEdit(user, editUserDashboardViewModel, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
        }
    }
}
