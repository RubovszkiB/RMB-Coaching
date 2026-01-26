using System.Collections.Generic;

public partial class Coach
{
    public int CoachId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public string? ProfileImage { get; set; }
    public byte IsActive { get; set; }

    public virtual ICollection<TrainingPlan> TrainingPlans { get; set; } = new List<TrainingPlan>();
}
