using System.ComponentModel.DataAnnotations;

namespace GameInventoryAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; }

        public List<Inventory> Inventories { get; set; }
    }
}