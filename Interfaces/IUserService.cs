
using System.Threading.Tasks;
using UserManagementAPI.Models;
using UserManagementAPI.Models.UserModels;

namespace UserManagementAPI.Services
{
    public interface IUserService
    {
        Task<ResponseResult<List<User>>> GetAllUsers();
        Task<ResponseResult<User>> GetUserById(int id);
    }
}
