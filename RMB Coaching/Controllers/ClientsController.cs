using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly WebshopdbContext _context;

    public ClientsController(WebshopdbContext context)
    {
        _context = context;
    }

    // POST: api/clients/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] ClientRegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            return StatusCode(400, new { message = "FullName, Email, Password required" });
        }

        var emailExists = await _context.Clients.AnyAsync(c => c.Email == dto.Email);
        if (emailExists)
            return StatusCode(409, new { message = "Email already exists" });

        var client = new Client
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = dto.Password, // !!! most sima jelszót tárolunk
            Phone = dto.Phone,
            Role = "client",
            IsActive = 1
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return StatusCode(201, new
        {
            message = "Registered",
            result = new
            {
                client.ClientId,
                client.FullName,
                client.Email,
                client.Phone,
                client.Role
            }
        });
    }

    // POST: api/clients/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] ClientLoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return StatusCode(400, new { message = "Email and Password required" });

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Email == dto.Email && c.IsActive == 1);

        if (client == null)
            return StatusCode(401, new { message = "Invalid credentials" });

        // !!! sima string összehasonlítás
        if (client.PasswordHash != dto.Password)
            return StatusCode(401, new { message = "Invalid credentials" });

        return StatusCode(200, new
        {
            message = "Logged in",
            result = new
            {
                client.ClientId,
                client.FullName,
                client.Email,
                client.Phone,
                client.Role
            }
        });
    }

    // GET: api/clients/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var client = await _context.Clients
            .Where(c => c.ClientId == id && c.IsActive == 1)
            .Select(c => new
            {
                c.ClientId,
                c.FullName,
                c.Email,
                c.Phone,
                c.Role
            })
            .FirstOrDefaultAsync();

        if (client == null)
            return StatusCode(404, new { message = "Not found" });

        return StatusCode(200, new { result = client });
    }

    // PUT: api/clients/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClientUpdateDto dto)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == id && c.IsActive == 1);
        if (client == null)
            return StatusCode(404, new { message = "Not found" });

        if (string.IsNullOrWhiteSpace(dto.FullName))
            return StatusCode(400, new { message = "FullName required" });

        client.FullName = dto.FullName;
        client.Phone = dto.Phone;

        await _context.SaveChangesAsync();
        return StatusCode(200, new { message = "Updated" });
    }

    // DELETE: api/clients/1 (soft delete)
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == id);
        if (client == null)
            return StatusCode(404, new { message = "Not found" });

        client.IsActive = 0;
        await _context.SaveChangesAsync();

        return StatusCode(200, new { message = "Deactivated" });
    }
}
