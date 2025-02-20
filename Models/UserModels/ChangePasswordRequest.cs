using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models.AuthModels
{
    public class ChangePasswordRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public required string OldPassword { get; set; }

        [Required]
        public required string NewPassword { get; set; }
    }
}
