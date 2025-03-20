using Microsoft.EntityFrameworkCore;
using GameInventoryAPI.Models; // Для User и Inventory
using GameInventoryAPI.Models.Items; // Для Item и его наследников

namespace GameInventoryAPI.Data
{
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
            modelBuilder.Entity<Item>()
                .HasDiscriminator<string>("Type")
                .HasValue<Weapon>("Weapon")
                .HasValue<Armor>("Armor")
                .HasValue<Jewelry>("Jewelry");

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
}