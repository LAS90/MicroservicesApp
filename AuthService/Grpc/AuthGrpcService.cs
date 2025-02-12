using Grpc.Core;
using AuthService.Services;
using AuthService.DTOs;
using AuthService.Interfaces;

namespace AuthService.Grpc
{
    public class AuthGrpcService : AuthService.AuthServiceBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthGrpcService(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            var dto = new RegisterUserDto
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _userService.RegisterUser(dto);

            return new RegisterResponse
            {
                Success = result,
                Message = result ? "Регистрация успешна" : "Email уже используется"
            };
        }

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var user = await _userService.GetUserByEmail(request.Email);

            if (user == null || !_userService.VerifyPassword(user, request.Password))
            {
                return new LoginResponse
                {
                    Token = "",
                    Message = "Неверный email или пароль"
                };
            }

            var token = _jwtService.GenerateToken(user.Email);

            return new LoginResponse
            {
                Token = token,
                Message = "Авторизация успешна"
            };
        }
    }

}
