using RunGroops.Data;
using RunGroops.Interfaces;
using RunGroops.Models;

namespace RunGroops.Repository {
    public class DashboardRepository : IDashboardRepository {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Club>> GetAllUserClubs() {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = _context.Clubs.Where(c => c.AppUser.Id == currentUser);
            return userClubs.ToList();
        }
        public async Task<List<Race>> GetAllUserRaces() {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userRaces = _context.Races.Where(r => r.AppUser.Id == curUser);
            return userRaces.ToList();
        }
    }
}
