using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models.AuthModels
{
    public class LoginRequest
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
