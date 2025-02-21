using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagementAPI.Models.AuthModels;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest loginRequest)
        {
            var response = await _authService.SignIn(loginRequest);
            return response.Success ? Ok(response) : Unauthorized(response);
        }
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var response = await _authService.ChangePassword(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
