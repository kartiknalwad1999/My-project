using EmployeeServiceDepartmentService.data_access;
using EmployeeServiceDepartmentService.domains;
using EmployeeServiceDepartmentService.Entity;
using EmployeeServiceDepartmentService.interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeServiceDepartmentService.Business_Handler
{
    public class AddEmployeeHandler : IRequestHandler<EmployeeRequestDto, EmployeeResponseDto>
    {
        private readonly IGenericRepository<DbContext, Employees> _employeeRepo;
        private readonly ILogger<AddEmployeeHandler> _logger;
        private readonly IUnitOfWork<EmpDepartmentDbContext> _unitOfWork;

        public AddEmployeeHandler(
            IGenericRepository<DbContext, Employees> employeeRepo,
            ILogger<AddEmployeeHandler> logger,
            IUnitOfWork<EmpDepartmentDbContext> unitOfWork)
        {
            _employeeRepo = employeeRepo;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<EmployeeResponseDto> Handle(EmployeeRequestDto request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting add operation for employee {Email}", request.Email);

            try
            {
                // 1. Check if employee already exists by email
                var existing = _employeeRepo.GetEmployees()
                                            .FirstOrDefault(e => e.Email == request.Email);
                if (existing != null)
                {
                    _logger.LogWarning("Employee with email {Email} already exists", request.Email);

                    return new EmployeeResponseDto
                    {
                        Success = false,
                        Message = $"Employee with email '{request.Email}' already exists."
                    };
                }

                // 2. Create new employee
                var employee = new Employees
                {
                    EmployeeId = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone,
                    HireDate = request.HireDate,
                    Status = request.Status
                };

                await _employeeRepo.AddAsync(employee);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Employee {Email} created with Id {EmployeeId}", request.Email, employee.EmployeeId);

                // 3. Build response
                return new EmployeeResponseDto
                {
                    EmployeeId = employee.EmployeeId,
                    Success = true,
                    Message = $"Employee '{employee.FirstName} {employee.LastName}' added successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding employee {Email}", request.Email);

                return new EmployeeResponseDto
                {
                    Success = false,
                    Message = $"An error occurred while adding employee '{request.Email}'."
                };
            }
        }
    }

}
