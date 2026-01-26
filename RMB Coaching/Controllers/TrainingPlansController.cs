using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/trainingplans")]
public class TrainingPlansController : ControllerBase
{
    private readonly WebshopdbContext _context;

    public TrainingPlansController(WebshopdbContext context)
    {
        _context = context;
    }

    // GET: api/trainingplans?category=football&difficulty=beginner&minPrice=0&maxPrice=10000&coachId=2
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? category,
        [FromQuery] string? difficulty,
        [FromQuery] int? minPrice,
        [FromQuery] int? maxPrice,
        [FromQuery] int? coachId)
    {
        var q = _context.TrainingPlans
            .Include(p => p.Coach)
            .AsQueryable();

        // csak publikált
        q = q.Where(p => p.IsPublished == 1);

        if (!string.IsNullOrWhiteSpace(category))
            q = q.Where(p => p.Category == category);

        // ha nincs difficulty oszlopod a DB-ben, ezt hagyd ki (nálad lehet nincs benne a manuális modelben)
        if (!string.IsNullOrWhiteSpace(difficulty))
            q = q.Where(p => EF.Property<string>(p, "Difficulty") == difficulty);

        if (minPrice.HasValue)
            q = q.Where(p => p.PriceHuf >= minPrice.Value);

        if (maxPrice.HasValue)
            q = q.Where(p => p.PriceHuf <= maxPrice.Value);

        if (coachId.HasValue)
            q = q.Where(p => p.CoachId == coachId.Value);

        var plans = await q.ToListAsync();
        return StatusCode(200, new { result = plans });
    }

    // GET: api/trainingplans/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var plan = await _context.TrainingPlans
            .Include(p => p.Coach)
            .FirstOrDefaultAsync(p => p.PlanId == id);

        if (plan == null)
            return StatusCode(404, new { message = "Not found" });

        return StatusCode(200, new { result = plan });
    }

    // POST: api/trainingplans
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TrainingPlan plan)
    {
        // minimál ellenőrzések
        if (string.IsNullOrWhiteSpace(plan.Title))
            return StatusCode(400, new { message = "Title is required" });

        var coachExists = await _context.Coaches.AnyAsync(c => c.CoachId == plan.CoachId && c.IsActive == 1);
        if (!coachExists)
            return StatusCode(400, new { message = "Invalid coachId" });

        // ha nem adták meg, legyen publikált
        if (plan.IsPublished == 0) plan.IsPublished = 1;

        _context.TrainingPlans.Add(plan);
        await _context.SaveChangesAsync();

        return StatusCode(201, new { result = plan });
    }

    // PUT: api/trainingplans/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TrainingPlan updated)
    {
        var plan = await _context.TrainingPlans.FirstOrDefaultAsync(p => p.PlanId == id);
        if (plan == null)
            return StatusCode(404, new { message = "Not found" });

        // mezők frissítése
        plan.Title = updated.Title;
        plan.Description = updated.Description;
        plan.Category = updated.Category;
        plan.PriceHuf = updated.PriceHuf;
        plan.IsPublished = updated.IsPublished;

        // coach csere opcionális (ha kell)
        if (updated.CoachId != 0 && updated.CoachId != plan.CoachId)
        {
            var coachExists = await _context.Coaches.AnyAsync(c => c.CoachId == updated.CoachId && c.IsActive == 1);
            if (!coachExists)
                return StatusCode(400, new { message = "Invalid coachId" });

            plan.CoachId = updated.CoachId;
        }

        await _context.SaveChangesAsync();
        return StatusCode(200, new { message = "Updated" });
    }

    // DELETE: api/trainingplans/5  (soft delete = elrejtés)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var plan = await _context.TrainingPlans.FirstOrDefaultAsync(p => p.PlanId == id);
        if (plan == null)
            return StatusCode(404, new { message = "Not found" });

        plan.IsPublished = 0;
        await _context.SaveChangesAsync();

        return StatusCode(200, new { message = "Hidden" });
    }
}
