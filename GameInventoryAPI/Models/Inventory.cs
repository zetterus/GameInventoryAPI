using System.ComponentModel.DataAnnotations;

public class Inventory
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int ItemId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1; // По умолчанию 1

    [Required]
    public DateTime AcquiredDate { get; set; } = DateTime.UtcNow;

    // Навигационные свойства
    public User User { get; set; }
    public Item Item { get; set; }
}
