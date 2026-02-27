using TappApi.Interfaces;
using TappApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TappApi.Repositories {
    public class UserRepository : IUserInterface {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllAsync() {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetByIdAsync(int id) {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User> CreateAsync(User user) {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> UpdateAsync(User user) {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id) {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}