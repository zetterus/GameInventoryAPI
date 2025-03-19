using System.ComponentModel.DataAnnotations;

public class Weapon : Item
{
    [Required]
    public int Damage { get; set; }

    [Required]
    public string WeaponType { get; set; } // "Sword", "Bow", "Axe"
}