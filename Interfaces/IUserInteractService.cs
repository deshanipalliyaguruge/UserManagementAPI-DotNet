using System.Threading.Tasks;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public interface IUserInteractService
    {
        Task<ResponseResult> CreateUser(User user);
        Task<ResponseResult> DeleteUser(int id);
    }
}
