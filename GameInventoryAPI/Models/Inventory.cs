public class Inventory
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; }
    public DateTime AcquiredDate { get; set; }
}