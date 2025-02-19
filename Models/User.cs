using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Got a Null or Invalid Value for {nameof(Name)}")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Got a Null or Invalid Value for {nameof(Email)}")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Got a Null or Invalid Value for {nameof(Age)}")]
        [Range(0, 100, ErrorMessage = "Age must be between 0 and 100")]
        public int Age { get; set; }

        public string? Gender { get; set; }

        [Required(ErrorMessage = "Got a Null or Invalid Value for {nameof(ContactNumber)}")]
        [Phone(ErrorMessage = "Invalid Contact Number")]
        public required string ContactNumber { get; set; }
    }
}
