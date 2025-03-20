using System.ComponentModel.DataAnnotations;
using GameInventoryAPI.Models.Items; // Для Item

namespace GameInventoryAPI.Models
{
    public class Inventory
    {
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime AcquiredDate { get; set; }
    }
}