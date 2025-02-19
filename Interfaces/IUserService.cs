using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public interface IUserService
    {
        Task<UserResponse<List<User>>> GetAllUsers();
        Task<UserResponse<User>> GetUserById(int id);
    }
}
