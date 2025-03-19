using System.ComponentModel.DataAnnotations;

public abstract class Item
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [MaxLength(8)]
    public string Type { get; set; } // "Weapon", "Armor", "Jewelry"

    [Required]
    public int Level { get; set; }

    [Required]
    public string Rarity { get; set; }

    public string UniqueProperties { get; set; }
}