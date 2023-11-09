using Microsoft.AspNetCore.Mvc;
using RunGroops.Data;
using RunGroops.Interfaces;
using RunGroops.ViewModels;

namespace RunGroops.Controllers {
    public class DashboardController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardController(ApplicationDbContext context, IDashboardRepository dashboardRepository) {
            _context = context;
            _dashboardRepository = dashboardRepository;
        }
        public async Task<IActionResult> Index() {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel() { Races = userRaces, Clubs = userClubs };
            return View(dashboardViewModel);
        }
    }
}
