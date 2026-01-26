using System.Collections.Generic;

public partial class Client
{
    public int ClientId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? Phone { get; set; }
    public byte IsActive { get; set; }
    public string Role { get; set; } = "client";

    public virtual ICollection<ClientTrainingPlan> ClientTrainingPlans { get; set; } = new List<ClientTrainingPlan>();
}
