using TappApi.Interfaces;
using TappApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TappApi.Repositories {
    public class TaskRepository : ITaskInterface {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync() {
            return await _context.Tasks
                .Include(t => t.User)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id) {
            return await _context.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskItem> CreateAsync(TaskItem task) {
            task.CreatedAt = DateTime.UtcNow;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> UpdateAsync(TaskItem task) {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id) {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) {
                return false;
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
