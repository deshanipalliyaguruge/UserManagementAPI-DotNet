using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagementAPI.Models.UserModels;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserInteractController : ControllerBase
    {
        private readonly IUserInteractService userService;

        public UserInteractController(IUserInteractService userService)
        {
            this.userService = userService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var response = await userService.CreateUser(user);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await userService.DeleteUser(id);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
