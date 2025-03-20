using GameInventoryAPI.Data;
using GameInventoryAPI.Models.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameInventoryAPI.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AddItemModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddItemModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string ItemType { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public int Level { get; set; }

        [BindProperty]
        public string Rarity { get; set; }

        [BindProperty]
        public string UniqueProperties { get; set; }

        [BindProperty]
        public int Damage { get; set; }

        [BindProperty]
        public string WeaponType { get; set; }

        [BindProperty]
        public int Defense { get; set; }

        [BindProperty]
        public string ArmorType { get; set; }

        [BindProperty]
        public int MagicPower { get; set; }

        [BindProperty]
        public string Effect { get; set; }

        public SelectList ItemTypes { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            ItemTypes = new SelectList(new[] { "Weapon", "Armor", "Jewelry" });
        }

        public IActionResult OnPost()
        {
            // Проверяем обязательные поля
            if (string.IsNullOrEmpty(ItemType) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Rarity) || Level <= 0)
            {
                ItemTypes = new SelectList(new[] { "Weapon", "Armor", "Jewelry" });
                Message = "Please fill in all required fields.";
                return Page();
            }

            // Дополнительные проверки для каждого типа
            if (ItemType == "Weapon" && (Damage <= 0 || string.IsNullOrEmpty(WeaponType)))
            {
                ItemTypes = new SelectList(new[] { "Weapon", "Armor", "Jewelry" });
                Message = "Please fill in all required fields for Weapon.";
                return Page();
            }

            if (ItemType == "Armor" && (Defense <= 0 || string.IsNullOrEmpty(ArmorType)))
            {
                ItemTypes = new SelectList(new[] { "Weapon", "Armor", "Jewelry" });
                Message = "Please fill in all required fields for Armor.";
                return Page();
            }

            if (ItemType == "Jewelry" && (MagicPower <= 0 || string.IsNullOrEmpty(Effect)))
            {
                ItemTypes = new SelectList(new[] { "Weapon", "Armor", "Jewelry" });
                Message = "Please fill in all required fields for Jewelry.";
                return Page();
            }

            // Создаём объект Item
            Item item = ItemType switch
            {
                "Weapon" => new Weapon
                {
                    Type = "Weapon",
                    Name = Name!, // Используем !, так как мы проверили Name на null
                    Level = Level,
                    Rarity = Rarity!, // Используем !, так как мы проверили Rarity на null
                    UniqueProperties = UniqueProperties ?? string.Empty,
                    Damage = Damage,
                    WeaponType = WeaponType! // Используем !, так как мы проверили WeaponType на null
                },
                "Armor" => new Armor
                {
                    Type = "Armor",
                    Name = Name!,
                    Level = Level,
                    Rarity = Rarity!,
                    UniqueProperties = UniqueProperties ?? string.Empty,
                    Defense = Defense,
                    ArmorType = ArmorType! // Используем !, так как мы проверили ArmorType на null
                },
                "Jewelry" => new Jewelry
                {
                    Type = "Jewelry",
                    Name = Name!,
                    Level = Level,
                    Rarity = Rarity!,
                    UniqueProperties = UniqueProperties ?? string.Empty,
                    MagicPower = MagicPower,
                    Effect = Effect! // Используем !, так как мы проверили Effect на null
                },
                _ => null
            };

            if (item == null)
            {
                ItemTypes = new SelectList(new[] { "Weapon", "Armor", "Jewelry" });
                Message = "Invalid item type.";
                return Page();
            }

            _context.Items.Add(item);
            _context.SaveChanges();

            Message = "Item added successfully!";
            ItemTypes = new SelectList(new[] { "Weapon", "Armor", "Jewelry" });
            return Page();
        }
    }
}