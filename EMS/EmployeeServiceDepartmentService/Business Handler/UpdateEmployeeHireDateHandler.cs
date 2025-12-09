using EmployeeServiceDepartmentService.data_access;
using EmployeeServiceDepartmentService.domains;
using EmployeeServiceDepartmentService.Entity;
using EmployeeServiceDepartmentService.interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeServiceDepartmentService.Business_Handler
{
    public class UpdateEmployeeHireDateHandler : IRequestHandler<EmployeeUpdateRequestDto, EmployeeUpdateResponseDto>
    {
        private readonly IGenericRepository<DbContext, Employees> _employeeRepo;
        private readonly ILogger<UpdateEmployeeHireDateHandler> _logger;
        private readonly IUnitOfWork<EmpDepartmentDbContext> _unitOfWork;

        public UpdateEmployeeHireDateHandler(
            IGenericRepository<DbContext, Employees> employeeRepo,
            ILogger<UpdateEmployeeHireDateHandler> logger,
            IUnitOfWork<EmpDepartmentDbContext> unitOfWork)
        {
            _employeeRepo = employeeRepo;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<EmployeeUpdateResponseDto> Handle(EmployeeUpdateRequestDto request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting update of HireDate for employee {EmployeeId}", request.EmployeeNumber);

            try
            {
                // 1. Find employee by Id
                var employee = await _employeeRepo.GetEmployees()
                                                  .FirstOrDefaultAsync(e => e.EmployeeNumber == request.EmployeeNumber, cancellationToken);

                if (employee == null)
                {
                    _logger.LogWarning("Employee with Id {EmployeeId} not found", request.EmployeeNumber);

                    return new EmployeeUpdateResponseDto
                    {
                        Success = false,
                        Message = $"Employee with Id '{request.EmployeeNumber}' not found."
                    };
                }

                // 2. Update only HireDate
                employee.HireDate = request.HireDate;
              

                await _employeeRepo.UpdateHireDateAsync(employee);
                await _unitOfWork.SaveChangesAsync();


                _logger.LogInformation("Employee {EmployeeId} HireDate updated to {HireDate}", employee.EmployeeId, employee.HireDate);

                // 3. Build response
                return new EmployeeUpdateResponseDto
                {
                   // EmployeeId = employee.EmployeeId,
                    Success = true,
                    Message = $"Employee '{employee.FirstName} {employee.LastName}' HireDate updated successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating HireDate for employee {EmployeeId}", request.EmployeeNumber);

                return new EmployeeUpdateResponseDto
                {
                    Success = false,
                    Message = $"An error occurred while updating HireDate for employee '{request.EmployeeNumber}'."
                };
            }
        }
    }
}
