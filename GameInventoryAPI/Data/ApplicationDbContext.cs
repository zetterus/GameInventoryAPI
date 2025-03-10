using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<Armor> Armors { get; set; }
    public DbSet<Jewelry> Jewelries { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка дискриминатора для Item (если используется TPH)
        modelBuilder.Entity<Item>()
            .HasDiscriminator<string>("Type")
            .HasValue<Weapon>("Weapon")
            .HasValue<Armor>("Armor")
            .HasValue<Jewelry>("Jewelry");

        // Настройка связей (если есть)
        modelBuilder.Entity<Inventory>()
            .HasKey(i => new { i.UserId, i.ItemId });

        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.User)
            .WithMany(u => u.Inventories)
            .HasForeignKey(i => i.UserId);

        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Item)
            .WithMany()
            .HasForeignKey(i => i.ItemId);
    }
}