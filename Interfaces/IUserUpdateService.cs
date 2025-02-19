using System.Threading.Tasks;
using UserManagementAPI.Models;
using UserManagementAPI.Models.UserModels;

namespace UserManagementAPI.Services
{
    public interface IUserUpdateService
    {
        Task<ResponseResult<object>> UpdateUser(int id, User user);
    }
}
