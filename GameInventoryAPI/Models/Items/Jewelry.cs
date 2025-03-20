using System.ComponentModel.DataAnnotations;

namespace GameInventoryAPI.Models.Items
{
    public class Jewelry : Item
    {
        [Required]
        public int MagicPower { get; set; }

        [Required]
        public string Effect { get; set; }
    }
}