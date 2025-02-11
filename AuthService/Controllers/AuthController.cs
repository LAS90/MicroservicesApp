using AuthService.DTOs;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly UserService _userService;

        public AuthController(JwtService jwtService, UserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _userService.RegisterUser(dto);
            if (!success)
                return Conflict(new { message = "Email уже зарегистрирован" });

            return Ok(new { message = "Пользователь зарегистрирован" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.GetUserByEmail(request.Email);
            if (user == null || !_userService.VerifyPassword(user, request.Password))
                return Unauthorized();

            var token = _jwtService.GenerateToken(request.Email);
            return Ok(new { token });
        }
    }
}
