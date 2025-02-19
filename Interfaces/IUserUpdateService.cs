using System.Threading.Tasks;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public interface IUserUpdateService
    {
        Task<ResponseResult<object>> UpdateUser(int id, User user);
    }
}
