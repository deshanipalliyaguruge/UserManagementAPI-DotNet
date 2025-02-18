using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.DTOs;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService service;

        public UserController(UserService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            return Ok(await service.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await service.GetUserById(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserDto userDto)
        {
            await service.AddUser(userDto);
            return CreatedAtAction(nameof(GetUser), new { id = userDto.Id }, userDto);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            var existingUser = await service.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();  // Return 404 if user does not exist
            }

            await service.UpdateUser(id, userDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await service.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();  // Return 404 if user does not exist
            }
            await service.DeleteUser(id);
            return NoContent();
        }
    }
}
