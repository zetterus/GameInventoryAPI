using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context, IConfiguration configuration)
    {
        // Удаляем базу данных, если она существует
        context.Database.EnsureDeleted();
        // Применение миграций (создание или обновление базы данных)
        context.Database.Migrate();

        // Чтение начальных данных из appsettings.json
        var initialData = configuration.GetSection("InitialData");

        // Заполнение таблицы Users
        if (!context.Users.Any())
        {
            var users = initialData.GetSection("Users").Get<List<User>>();
            context.Users.AddRange(users);
            context.SaveChanges();
        }

        // Заполнение таблицы Items
        if (!context.Items.Any())
        {
            var items = initialData.GetSection("Items").Get<List<Item>>();

            foreach (var itemData in items)
            {
                Item item = null;

                switch (itemData.Type)
                {
                    case "Weapon":
                        item = new Weapon
                        {
                            Name = itemData.Name,
                            Type = itemData.Type,
                            Level = itemData.Level,
                            Rarity = itemData.Rarity,
                            UniqueProperties = itemData.UniqueProperties ?? string.Empty, // Убедимся, что значение не NULL
                            Damage = (itemData as Weapon)?.Damage ?? 0,
                            WeaponType = (itemData as Weapon)?.WeaponType
                        };
                        break;

                    case "Armor":
                        item = new Armor
                        {
                            Name = itemData.Name,
                            Type = itemData.Type,
                            Level = itemData.Level,
                            Rarity = itemData.Rarity,
                            UniqueProperties = itemData.UniqueProperties ?? string.Empty, // Убедимся, что значение не NULL
                            Defense = (itemData as Armor)?.Defense ?? 0,
                            ArmorType = (itemData as Armor)?.ArmorType
                        };
                        break;

                    case "Jewelry":
                        item = new Jewelry
                        {
                            Name = itemData.Name,
                            Type = itemData.Type,
                            Level = itemData.Level,
                            Rarity = itemData.Rarity,
                            UniqueProperties = itemData.UniqueProperties ?? string.Empty, // Убедимся, что значение не NULL
                            MagicPower = (itemData as Jewelry)?.MagicPower ?? 0,
                            Effect = (itemData as Jewelry)?.Effect
                        };
                        break;
                }

                if (item != null)
                {
                    context.Items.Add(item);
                }
            }

            context.SaveChanges();
        }

        // Заполнение таблицы Inventories
        if (!context.Inventories.Any())
        {
            var user = context.Users.First(); // Берём первого пользователя
            var items = context.Items.Take(3).ToList(); // Берём первые 3 предмета

            foreach (var item in items)
            {
                context.Inventories.Add(new Inventory
                {
                    UserId = user.Id,
                    ItemId = item.Id,
                    AcquiredDate = DateTime.UtcNow // Указываем текущую дату
                });
            }

            context.SaveChanges();
        }
    }
}