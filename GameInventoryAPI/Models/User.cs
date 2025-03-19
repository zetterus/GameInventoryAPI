using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string Role { get; set; } = "User"; // По умолчанию "User"

    // Связь с инвентарём
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
