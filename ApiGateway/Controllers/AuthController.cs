using Grpc.Net.Client;
using AuthService.Grpc;
using Microsoft.AspNetCore.Mvc;
using Grpc.Core;
using static AuthService.Grpc.AuthService;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthServiceClient _authServiceClient;

        public AuthController(AuthServiceClient authServiceClient)
        {
            _authServiceClient = authServiceClient;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _authServiceClient.RegisterAsync(request);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (RpcException ex)
            {
                return StatusCode(500, ex.Status.Detail);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authServiceClient.LoginAsync(request);
                return response.Token != "" ? Ok(response) : Unauthorized(response);
            }
            catch (RpcException ex)
            {
                return StatusCode(500, ex.Status.Detail);
            }
        }
    }
}
