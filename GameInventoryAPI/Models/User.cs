public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } = "User";

    // Коллекция для связи с Inventory
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}