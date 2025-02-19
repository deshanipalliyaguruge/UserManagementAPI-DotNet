using System.Threading.Tasks;
using UserManagementAPI.Models;
using UserManagementAPI.Models.UserModels;

namespace UserManagementAPI.Services
{
    public interface IUserInteractService
    {
        Task<ResponseResult<object>> CreateUser(User user);
        Task<ResponseResult<object>> DeleteUser(int id);
    }
}
