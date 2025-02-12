using AuthService.DTOs;
using AuthService.Interfaces;
using AuthService.Models;
using AuthService.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUser(RegisterUserDto dto)
        {
            if (await _userRepository.UserExists(dto.Email))
                return false; // Email уже занят

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password)
            };

            await _userRepository.AddUser(user);
            return true;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public bool VerifyPassword(User user, string password)
        {
            return user.PasswordHash == HashPassword(password);
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
