using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagementAPI.Models.UserModels;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/user-update")]
    public class UserUpdateController : ControllerBase
    {
        private readonly IUserUpdateService _userUpdateService;

        public UserUpdateController(IUserUpdateService userUpdateService)
        {
            _userUpdateService = userUpdateService;
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var response = await _userUpdateService.UpdateUser(id, user);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
