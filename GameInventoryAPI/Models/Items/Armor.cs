using System.ComponentModel.DataAnnotations;

namespace GameInventoryAPI.Models.Items
{
    public class Armor : Item
    {
        [Required]
        public int Defense { get; set; }

        [Required]
        public string ArmorType { get; set; }
    }
}