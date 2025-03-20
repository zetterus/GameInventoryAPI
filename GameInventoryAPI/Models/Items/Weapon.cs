using System.ComponentModel.DataAnnotations;

namespace GameInventoryAPI.Models.Items
{
    public class Weapon : Item
    {
        [Required]
        public int Damage { get; set; }

        [Required]
        public string WeaponType { get; set; }
    }
}