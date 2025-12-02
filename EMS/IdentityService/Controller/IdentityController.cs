using MediatR;
using Microsoft.AspNetCore.Mvc;
using IdentityService.Domain;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;
  //  private readonly ILogger _logger;
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(IMediator mediator, ILogger<IdentityController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

   
    [HttpPost("register")]
    public async Task<ActionResult<RegisterUserResponseDto>> Register([FromBody] RegisterUserRequestDto request)
    {
        _logger.LogInformation("Received registration request for {Username}", request.Username);

        var response = await _mediator.Send(request);

        if (!response.Success)
        {
            // Return 400 Bad Request with message if registration failed
            return BadRequest(response);
        }

        // Return 200 OK with success response
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _mediator.Send(request);

        if (!response.Success)
            return Unauthorized(response);

        return Ok(response);
    }
}
