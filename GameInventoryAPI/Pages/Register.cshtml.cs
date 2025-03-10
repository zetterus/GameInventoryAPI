using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class RegisterModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public RegisterModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    [BindProperty]
    public string Role { get; set; }

    public string Message { get; set; }

    public void OnGet()
    {
        // Метод для отображения страницы
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Console.WriteLine("OnPostAsync called"); // Логирование
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Проверка, существует ли пользователь с таким именем
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == Username);
        if (existingUser != null)
        {
            Message = "Username already exists.";
            return Page();
        }

        // Создание нового пользователя
        var user = new User
        {
            Username = Username,
            PasswordHash = HashPassword(Password), // Хэшируем пароль
            Role = Role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        Message = "User registered successfully!";
        return Page();
    }

    private string HashPassword(string password)
    {
        // Простейший пример хэширования пароля (в реальном проекте используйте библиотеку, например, BCrypt)
        return password.GetHashCode().ToString();
    }
}