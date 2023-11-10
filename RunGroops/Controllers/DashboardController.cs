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
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            return View(editUserViewModel);
        }
    }
}
