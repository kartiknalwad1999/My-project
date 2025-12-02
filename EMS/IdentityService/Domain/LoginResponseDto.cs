using MediatR;

namespace IdentityService.Domain
{
    // Request DTO
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}