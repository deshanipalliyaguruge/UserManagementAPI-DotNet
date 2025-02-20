using System.Threading.Tasks;
using UserManagementAPI.Models.AuthModels;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public interface IAuthService
    {
        Task<ResponseResult<LoginResponse>> SignIn(LoginRequest loginRequest);
        Task<ResponseResult<object>> ChangePassword(ChangePasswordRequest request);
    }
}
