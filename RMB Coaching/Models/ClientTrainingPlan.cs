using System;

public partial class ClientTrainingPlan
{
    public int ClientId { get; set; }
    public int PlanId { get; set; }

    public DateTime PurchasedAt { get; set; }
    public int PricePaidHuf { get; set; }
    public string Status { get; set; } = "active";

    public virtual Client Client { get; set; } = null!;

    public virtual TrainingPlan TrainingPlan { get; set; } = null!;
}
