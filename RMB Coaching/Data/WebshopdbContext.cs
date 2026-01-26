using Microsoft.EntityFrameworkCore;

public partial class WebshopdbContext : DbContext
{
    public WebshopdbContext(DbContextOptions<WebshopdbContext> options) : base(options) { }

    public DbSet<Coach> Coaches => Set<Coach>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<TrainingPlan> TrainingPlans => Set<TrainingPlan>();
    public DbSet<ClientTrainingPlan> ClientTrainingPlans => Set<ClientTrainingPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // -------------------------
        // coaches
        // -------------------------
        modelBuilder.Entity<Coach>(entity =>
        {
            entity.ToTable("coaches");
            entity.HasKey(e => e.CoachId);

            entity.Property(e => e.CoachId).HasColumnName("coach_id");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.ProfileImage).HasColumnName("profile_image");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
        });

        // -------------------------
        // clients
        // -------------------------
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("clients");
            entity.HasKey(e => e.ClientId);

            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Role).HasColumnName("role");
        });

        // -------------------------
        // training_plans
        // -------------------------
        modelBuilder.Entity<TrainingPlan>(entity =>
        {
            entity.ToTable("training_plans");
            entity.HasKey(e => e.PlanId);

            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.CoachId).HasColumnName("coach_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.PriceHuf).HasColumnName("price_huf");
            entity.Property(e => e.IsPublished).HasColumnName("is_published");

            entity.HasOne(d => d.Coach)
                .WithMany(p => p.TrainingPlans)
                .HasForeignKey(d => d.CoachId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // -------------------------
        // client_training_plans (kapcsoló)
        // -------------------------
        modelBuilder.Entity<ClientTrainingPlan>(entity =>
        {
            entity.ToTable("client_training_plans");
            entity.HasKey(e => new { e.ClientId, e.PlanId });

            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.PurchasedAt).HasColumnName("purchased_at");
            entity.Property(e => e.PricePaidHuf).HasColumnName("price_paid_huf");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.ClientTrainingPlans)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.TrainingPlan)
                .WithMany(p => p.ClientTrainingPlans)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
