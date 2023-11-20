using Microsoft.EntityFrameworkCore;
using RunGroops.Data;
using RunGroops.Interfaces;
using RunGroops.Models;

namespace RunGroops.Repository {
    public class ClubRepository : IClubRepository {
        private readonly ApplicationDbContext _context;
        public ClubRepository(ApplicationDbContext context) {
            _context = context;
        }

        public bool Add(Club club) {
            _context.Add(club);
            return Save();
        }

        public bool Delete(Club club) {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll() {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id) {
            return await _context.Clubs.Include(c => c.Address).Include(c => c.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Club> GetByIdAsyncNoTracking(int id) {
            return await _context.Clubs.Include(c => c.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city) {
            return await _context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<AppUser> GetClubOwner(string appUserId) {
            return await _context.Users.FindAsync(appUserId);
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Club club) {
            _context.Update(club);
            return Save();
        }
    }
}
