using TappApi.Models;
using System.Collections.Generic;

namespace TappApi.Interfaces {
    public interface ITaskInterface {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem task);
        Task<bool> UpdateAsync(TaskItem task);
        Task<bool> DeleteAsync(int id);
    }
}
