using AuthService.Models;
using AuthService.DTOs;

namespace AuthService.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUser(RegisterUserDto dto);
        Task<User?> GetUserByEmail(string email);
        bool VerifyPassword(User user, string password);
        string HashPassword(string password);
    }
}
