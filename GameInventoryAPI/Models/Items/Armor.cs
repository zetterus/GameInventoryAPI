using System.ComponentModel.DataAnnotations;

public class Armor : Item
{
    [Required]
    public int Defense { get; set; }

    [Required]
    public string ArmorType { get; set; } // "Helmet", "Chestplate"
}