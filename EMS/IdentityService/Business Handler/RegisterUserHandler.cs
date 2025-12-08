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
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequestDto, RegisterUserResponseDto>
    {
        private readonly IGenericRepository<DbContext, User> _userRepo;
        private readonly IGenericRepository<DbContext, Role> _roleRepo;
        private readonly ILogger<RegisterUserHandler> _logger;
        private readonly IUnitOfWork<IdentityDbContext> _unitOfWork;

        public RegisterUserHandler(
            IGenericRepository<DbContext, User> userRepo,
            IGenericRepository<DbContext, Role> roleRepo,
            ILogger<RegisterUserHandler> logger,
            IUnitOfWork<IdentityDbContext> unitOfWork)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterUserResponseDto> Handle(RegisterUserRequestDto request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting registration for user {Username}", request.Username);

            try
            {
                // 1. Check if user already exists
                var existingUsers = _userRepo.GetUsers()
                                             .Where(u => u.Username == request.Username)
                                             .ToList();
                if (existingUsers.Any())
                {
                    _logger.LogWarning("User {Username} already exists", request.Username);

                    return new RegisterUserResponseDto
                    {
                        Success = false,
                        Message = $"User '{request.Username}' already exists."
                    };
                }

                // 2. Validate role
                var role = _roleRepo.GetRoles()
                                    .FirstOrDefault(r => r.Name == request.Role);

                if (role == null)
                {
                    _logger.LogWarning("Role {Role} not found", request.Role);

                    return new RegisterUserResponseDto
                    {
                        Success = false,
                        Message = $"Role '{request.Role}' not found."
                    };
                }

                // 3. Create user with RoleId directly
                var user = new User
                {
                    Username = request.Username,
                    PasswordHash = request.Password, // ⚠️ hash in real apps
                    CreatedAt = DateTime.UtcNow,
                    RoleId = role.RoleId   // assign role directly
                };

                await _userRepo.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("User {Username} created with Id {UserId} and Role {Role}",
                                        user.Username, user.UserId, role.Name);

                // 4. Build response
                return new RegisterUserResponseDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                  //  EmployeeId = user.EmployeeId,
                    CreatedAt = user.CreatedAt,
                    Role = role.Name,
                    Success = true,
                    Message = $"User '{user.Username}' registered successfully with role '{role.Name}'."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user {Username}", request.Username);

                return new RegisterUserResponseDto
                {
                    Success = false,
                    Message = $"An error occurred while registering user '{request.Username}'."
                };
            }
        }
    }
}
