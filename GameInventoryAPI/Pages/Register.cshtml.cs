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
        // ����� ��� ����������� ��������
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Console.WriteLine("OnPostAsync called"); // �����������
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // ��������, ���������� �� ������������ � ����� ������
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == Username);
        if (existingUser != null)
        {
            Message = "Username already exists.";
            return Page();
        }

        // �������� ������ ������������
        var user = new User
        {
            Username = Username,
            PasswordHash = HashPassword(Password), // �������� ������
            Role = Role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        Message = "User registered successfully!";
        return Page();
    }

    private string HashPassword(string password)
    {
        // ���������� ������ ����������� ������ (� �������� ������� ����������� ����������, ��������, BCrypt)
        return password.GetHashCode().ToString();
    }
}