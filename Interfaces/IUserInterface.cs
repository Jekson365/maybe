using TappApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TappApi.Interfaces {
    public interface IUserInterface
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
    }
}
