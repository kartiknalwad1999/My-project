using IdentityService.Data;
using IdentityService.Domain;
using IdentityService.Entities;
using IdentityService.Infrastructure;
using IdentityService.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityService.Business_Handler
{
    public class LoginHandler : IRequestHandler<LoginRequestDto, LoginResponseDto>
    {
        private readonly IGenericRepository<DbContext, User> _userRepo;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(
            IGenericRepository<DbContext, User> userRepo,
            ILogger<LoginHandler> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }

        public async Task<LoginResponseDto> Handle(LoginRequestDto request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login attempt for user {Username}", request.Username);

            try
            {
                // ✅ Check username and password together
                var user = _userRepo.GetUsers()
                                    .FirstOrDefault(u => u.Username == request.Username
                                                      && u.PasswordHash == request.Password);

                if (user == null)
                {
                    _logger.LogWarning("Invalid login attempt for user {Username}", request.Username);
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Invalid username or password."
                    };
                }

                // ✅ Both username and password matched
                _logger.LogInformation("User {Username} logged in successfully", user.Username);

                return new LoginResponseDto
                {
                    Success = true,
                    Message = "Login successful.",
                    //UserId = user.UserId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in user {Username}", request.Username);
                return new LoginResponseDto
                {
                    Success = false,
                    Message = $"An error occurred while logging in user '{request.Username}'."
                };
            }
        }
    }
}
