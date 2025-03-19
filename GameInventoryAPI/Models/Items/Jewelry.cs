using System.ComponentModel.DataAnnotations;

public class Jewelry : Item
{
    [Required]
    public int MagicPower { get; set; }

    [Required]
    public string Effect { get; set; } // "+10 Mana Regen"
}