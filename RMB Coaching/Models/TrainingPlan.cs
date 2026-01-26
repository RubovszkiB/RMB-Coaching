using System.Collections.Generic;

public partial class TrainingPlan
{
    public int PlanId { get; set; }
    public int CoachId { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Category { get; set; } = null!;
    public int PriceHuf { get; set; }
    public byte IsPublished { get; set; }
    public virtual Coach Coach { get; set; } = null!;

    public virtual ICollection<ClientTrainingPlan> ClientTrainingPlans { get; set; } = new List<ClientTrainingPlan>();
}
