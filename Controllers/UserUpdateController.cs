using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagementAPI.Models;
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

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            var response = await _userUpdateService.UpdateUser(id, user);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
