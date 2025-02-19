
using System.Threading.Tasks;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public interface IUserService
    {
        Task<ResponseResult<List<User>>> GetAllUsers();
        Task<ResponseResult<User>> GetUserById(int id);
    }
}
