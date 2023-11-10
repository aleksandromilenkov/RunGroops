using RunGroops.Models;

namespace RunGroops.Interfaces {
    public interface IDashboardRepository {
        Task<List<Race>> GetAllUserRaces();
        Task<List<Club>> GetAllUserClubs();
        Task<AppUser> GetUserById(string id);
    }
}
