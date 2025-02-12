using Grpc.Core;
using AuthService.Services;
using AuthService.DTOs;
using AuthService.Interfaces;

namespace AuthService.Grpc
{
    public class AuthGrpcService : AuthService.AuthServiceBase
    {
        private readonly ILogger<AuthGrpcService> _logger;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthGrpcService(IUserService userService, IJwtService jwtService, ILogger<AuthGrpcService> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }

        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            try
            {
                _logger.LogInformation("Попытка регистрации пользователя с email: {Email}", request.Email);

                var dto = new RegisterUserDto
                {
                    Email = request.Email,
                    Password = request.Password
                };

                var result = await _userService.RegisterUser(dto);

                if (result)
                {
                    _logger.LogInformation("Регистрация успешна для email: {Email}", request.Email);
                    return new RegisterResponse
                    {
                        Success = true,
                        Message = "Регистрация успешна"
                    };
                }
                else
                {
                    _logger.LogWarning("Email {Email} уже используется", request.Email);
                    return new RegisterResponse
                    {
                        Success = false,
                        Message = "Email уже используется"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя с email: {Email}", request.Email);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка при регистрации пользователя"));
            }
        }

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            try
            {
                _logger.LogInformation("Попытка входа пользователя с email: {Email}", request.Email);

                var user = await _userService.GetUserByEmail(request.Email);

                if (user == null || !_userService.VerifyPassword(user, request.Password))
                {
                    _logger.LogWarning("Неверный email или пароль для email: {Email}", request.Email);
                    return new LoginResponse
                    {
                        Token = "",
                        Message = "Неверный email или пароль"
                    };
                }

                var token = _jwtService.GenerateToken(user.Email);

                _logger.LogInformation("Авторизация успешна для email: {Email}", request.Email);

                return new LoginResponse
                {
                    Token = token,
                    Message = "Авторизация успешна"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при авторизации пользователя с email: {Email}", request.Email);
                throw new RpcException(new Status(StatusCode.Internal, "Ошибка при авторизации пользователя"));
            }
        }
    }

}
