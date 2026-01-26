using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly WebshopdbContext _context;
    public HealthController(WebshopdbContext context) => _context = context;

    [HttpGet("db")]
    public async Task<IActionResult> Db()
    {
        var plans = await _context.TrainingPlans.CountAsync();
        var coaches = await _context.Coaches.CountAsync();
        return Ok(new { ok = true, plans, coaches });
    }
}
