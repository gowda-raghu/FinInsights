using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailiumService _emailService;

    public EmailController(IEmailiumService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] EmailDto dto)
    {
        await _emailService.SendEmailAsync(dto.To, dto.Subject, dto.Body);
        return Ok(true);
    }
}