using MediatR;
using System;

namespace IdentityService.Domain
{
    public class RegisterUserRequestDto : IRequest<RegisterUserResponseDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    
        public string Role { get; set; }
    }
}
