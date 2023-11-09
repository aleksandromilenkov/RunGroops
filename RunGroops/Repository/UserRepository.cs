using Microsoft.EntityFrameworkCore;
using RunGroops.Data;
using RunGroops.Interfaces;
using RunGroops.Models;

namespace RunGroops.Repository {
    public class UserRepository : IUserRepository {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) {
            this._context = context;
        }
        public bool Add(AppUser user) {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user) {
            _context.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers() {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string userId) {
            return await _context.Users.FindAsync(userId);
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user) {
            _context.Update(user);
            return Save();
        }
    }
}
