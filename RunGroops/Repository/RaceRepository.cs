using Microsoft.EntityFrameworkCore;
using RunGroops.Data;
using RunGroops.Interfaces;
using RunGroops.Models;

namespace RunGroops.Repository {
    public class RaceRepository : IRaceRepository {
        private readonly ApplicationDbContext _context;
        public RaceRepository(ApplicationDbContext context) {
            _context = context;

        }
        public bool Add(Race race) {
            _context.Add(race);
            return Save();
        }

        public bool Delete(Race club) {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll() {
            return await _context.Races.ToListAsync();
        }

        public async Task<Race> GetByIdAsync(int id) {
            return await _context.Races.Include(r => r.Address).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Race>> GetAllRacesByCity(string city) {
            return await _context.Races.Where(r => r.Address.City == city).ToListAsync();
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Race club) {
            _context.Update(club);
            return Save();
        }
    }
}
