using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/coaches")]
public class CoachesController : ControllerBase
{
    private readonly WebshopdbContext _context;

    public CoachesController(WebshopdbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var coaches = await _context.Coaches
            .Where(c => c.IsActive == 1)
            .ToListAsync();

        return StatusCode(200, new { result = coaches });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var coach = await _context.Coaches.FirstOrDefaultAsync(c => c.CoachId == id);
        if (coach == null) return StatusCode(404);

        return StatusCode(200, new { result = coach });
    }

    // GET: api/coaches/2/trainingplans
    [HttpGet("{id:int}/trainingplans")]
    public async Task<IActionResult> GetCoachPlans(int id)
    {
        var plans = await _context.TrainingPlans
            .Where(p => p.CoachId == id && p.IsPublished == 1)
            .ToListAsync();

        return StatusCode(200, new { result = plans });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Coach coach)
    {
        if (string.IsNullOrWhiteSpace(coach.FullName) || string.IsNullOrWhiteSpace(coach.Email))
            return StatusCode(400, new { message = "FullName and Email are required" });

        _context.Coaches.Add(coach);
        await _context.SaveChangesAsync();
        return StatusCode(201, new { result = coach });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Coach updated)
    {
        var coach = await _context.Coaches.FirstOrDefaultAsync(c => c.CoachId == id);
        if (coach == null) return StatusCode(404);

        coach.FullName = updated.FullName;
        coach.Email = updated.Email;
        coach.Phone = updated.Phone;
        coach.Bio = updated.Bio;
        coach.ProfileImage = updated.ProfileImage;
        coach.IsActive = updated.IsActive;

        await _context.SaveChangesAsync();
        return StatusCode(200, new { message = "Updated" });
    }

    // soft delete
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var coach = await _context.Coaches.FirstOrDefaultAsync(c => c.CoachId == id);
        if (coach == null) return StatusCode(404);

        coach.IsActive = 0;
        await _context.SaveChangesAsync();
        return StatusCode(200, new { message = "Deactivated" });
    }
}
