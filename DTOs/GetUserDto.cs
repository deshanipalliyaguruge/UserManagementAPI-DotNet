namespace UserManagementAPI.DTOs
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public required string ContactNumber { get; set; }
    }
}
