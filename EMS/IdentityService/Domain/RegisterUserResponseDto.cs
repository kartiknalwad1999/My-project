namespace IdentityService.Domain
{
    public class RegisterUserResponseDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; }

        // ✅ New fields
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
