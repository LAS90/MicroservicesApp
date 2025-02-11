using AuthService.Models;

namespace AuthService.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserExists(string email);
        Task AddUser(User user);
        Task<User?> GetUserByEmail(string email);
    }
}
