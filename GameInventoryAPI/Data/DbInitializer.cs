using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GameInventoryAPI.Models; // Для User и Inventory
using GameInventoryAPI.Models.Items; // Для Item и его наследников

namespace GameInventoryAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, IConfiguration configuration)
        {
            var recreateDatabase = configuration.GetValue<bool>("Database:RecreateOnStart");

            if (recreateDatabase)
            {
                context.Database.EnsureDeleted();
            }

            context.Database.Migrate();

            var initialData = configuration.GetSection("InitialData");

            if (!context.Users.Any())
            {
                var users = initialData.GetSection("Users").Get<List<User>>() ?? new List<User>();
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            if (!context.Items.Any())
            {
                var items = initialData.GetSection("Items").Get<List<Item>>() ?? new List<Item>();

                foreach (var itemData in items)
                {
                    Item item = itemData switch
                    {
                        Weapon weapon => new Weapon
                        {
                            Name = weapon.Name,
                            Level = weapon.Level,
                            Rarity = weapon.Rarity,
                            UniqueProperties = weapon.UniqueProperties ?? string.Empty,
                            Damage = weapon.Damage,
                            WeaponType = weapon.WeaponType,
                            Type = "Weapon"
                        },
                        Armor armor => new Armor
                        {
                            Name = armor.Name,
                            Level = armor.Level,
                            Rarity = armor.Rarity,
                            UniqueProperties = armor.UniqueProperties ?? string.Empty,
                            Defense = armor.Defense,
                            ArmorType = armor.ArmorType,
                            Type = "Armor"
                        },
                        Jewelry jewelry => new Jewelry
                        {
                            Name = jewelry.Name,
                            Level = jewelry.Level,
                            Rarity = jewelry.Rarity,
                            UniqueProperties = jewelry.UniqueProperties ?? string.Empty,
                            MagicPower = jewelry.MagicPower,
                            Effect = jewelry.Effect,
                            Type = "Jewelry"
                        },
                        _ => null
                    };

                    if (item != null)
                    {
                        context.Items.Add(item);
                    }
                }

                context.SaveChanges();
            }

            if (!context.Inventories.Any())
            {
                var user = context.Users.FirstOrDefault();
                var items = context.Items.Take(3).ToList();

                if (user != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        context.Inventories.Add(new Inventory
                        {
                            UserId = user.Id,
                            ItemId = item.Id,
                            Quantity = 1,
                            AcquiredDate = DateTime.UtcNow
                        });
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}