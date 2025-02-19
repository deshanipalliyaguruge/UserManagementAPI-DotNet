using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagementAPI.Models;
using UserManagementAPI.Services;
using System.Collections.Generic;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await userService.GetAllUsers();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await userService.GetUserById(id);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
