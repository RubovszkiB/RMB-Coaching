using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/purchases")]
public class PurchasesController : ControllerBase
{
    private readonly WebshopdbContext _context;

    public PurchasesController(WebshopdbContext context)
    {
        _context = context;
    }

    // POST: api/purchases
    // Body: { "clientId": 1, "planId": 3 }
    [HttpPost]
    public async Task<IActionResult> Buy([FromBody] PurchaseDto dto)
    {
        // validálás
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == dto.ClientId && c.IsActive == 1);
        if (client == null)
            return StatusCode(400, new { message = "Invalid clientId" });

        var plan = await _context.TrainingPlans.FirstOrDefaultAsync(p => p.PlanId == dto.PlanId && p.IsPublished == 1);
        if (plan == null)
            return StatusCode(400, new { message = "Invalid planId" });

        // már megvette?
        var exists = await _context.ClientTrainingPlans
            .AnyAsync(x => x.ClientId == dto.ClientId && x.PlanId == dto.PlanId && x.Status == "active");

        if (exists)
            return StatusCode(409, new { message = "Already purchased" });

        var purchase = new ClientTrainingPlan
        {
            ClientId = dto.ClientId,
            PlanId = dto.PlanId,
            PurchasedAt = DateTime.Now,
            PricePaidHuf = plan.PriceHuf,
            Status = "active"
        };

        _context.ClientTrainingPlans.Add(purchase);
        await _context.SaveChangesAsync();

        return StatusCode(201, new
        {
            message = "Purchased",
            result = purchase
        });
    }

    // GET: api/purchases/client/1
    [HttpGet("client/{clientId:int}")]
    public async Task<IActionResult> GetClientPurchases(int clientId)
    {
        var clientExists = await _context.Clients.AnyAsync(c => c.ClientId == clientId && c.IsActive == 1);
        if (!clientExists)
            return StatusCode(404, new { message = "Client not found" });

        var purchases = await _context.ClientTrainingPlans
            .Where(x => x.ClientId == clientId && x.Status == "active")
            .Include(x => x.TrainingPlan)
            .ThenInclude(p => p.Coach)
            .ToListAsync();

        // Reactnak kényelmesebb egy “listázott” struktúra
        var result = purchases.Select(x => new
        {
            x.ClientId,
            x.PlanId,
            x.PurchasedAt,
            x.PricePaidHuf,
            x.Status,
            plan = new
            {
                x.TrainingPlan.PlanId,
                x.TrainingPlan.Title,
                x.TrainingPlan.Category,
                x.TrainingPlan.PriceHuf,
                coach = new
                {
                    x.TrainingPlan.Coach.CoachId,
                    x.TrainingPlan.Coach.FullName
                }
            }
        });

        return StatusCode(200, new { result });
    }

    // GET: api/purchases/plan/3  (admin/statisztika jelleg)
    [HttpGet("plan/{planId:int}")]
    public async Task<IActionResult> GetPlanPurchases(int planId)
    {
        var planExists = await _context.TrainingPlans.AnyAsync(p => p.PlanId == planId);
        if (!planExists)
            return StatusCode(404, new { message = "Plan not found" });

        var buyers = await _context.ClientTrainingPlans
            .Where(x => x.PlanId == planId && x.Status == "active")
            .Include(x => x.Client)
            .ToListAsync();

        var result = buyers.Select(x => new
        {
            x.PlanId,
            x.ClientId,
            x.PurchasedAt,
            x.PricePaidHuf,
            client = new
            {
                x.Client.ClientId,
                x.Client.FullName,
                x.Client.Email
            }
        });

        return StatusCode(200, new { result });
    }

    // DELETE: api/purchases (visszavonás / refund jelleg)
    // Body: { "clientId": 1, "planId": 3 }
    [HttpDelete]
    public async Task<IActionResult> Revoke([FromBody] PurchaseDto dto)
    {
        var purchase = await _context.ClientTrainingPlans
            .FirstOrDefaultAsync(x => x.ClientId == dto.ClientId && x.PlanId == dto.PlanId);

        if (purchase == null)
            return StatusCode(404, new { message = "Purchase not found" });

        purchase.Status = "revoked";
        await _context.SaveChangesAsync();

        return StatusCode(200, new { message = "Revoked" });
    }
}
