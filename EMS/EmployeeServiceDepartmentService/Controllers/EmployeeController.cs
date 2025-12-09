using EmployeeServiceDepartmentService.domains;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeServiceDepartmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IMediator mediator, ILogger<EmployeeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

      
        [HttpPost("add")]
        public async Task<ActionResult> AddEmployee([FromBody] EmployeeRequestDto request)
        {
            if (request == null)
            {
                return BadRequest(new { Message = "Invalid request payload." });
            }

            try
            {
                var response = await _mediator.Send(request);
                if (!response.Success)
                {
                    _logger.LogWarning("Failed to add employee: {Message}", response.Message);
                    return BadRequest(response);
                }

                _logger.LogInformation("Employee added successfully with Id {EmployeeId}", response.EmployeeId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding employee");
                return StatusCode(500, new { Message = "Internal server error occurred." });
            }
        }
        [HttpPatch("update-hiredate")]
        public async Task<IActionResult> UpdateHireDate([FromBody] EmployeeUpdateRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request payload.");
            }

            _logger.LogInformation("Received request to update HireDate for employee {EmployeeNumber}", request.EmployeeNumber);

            var response = await _mediator.Send(request);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }

}
