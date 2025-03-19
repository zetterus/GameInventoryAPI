using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GameInventoryAPI.Pages
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public RegisterModel(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        [BindProperty]
        [Required, MinLength(3), MaxLength(30)]
        public string Username { get; set; }

        [BindProperty]
        [Required, MinLength(6)]
        public string Password { get; set; }

        // ������ �������� Role, ��� ��� ��� ������ �� ����� �� �����
        public string Message { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == Username);
            if (existingUser != null)
            {
                Message = "Username already exists.";
                return Page();
            }

            var user = new User
            {
                Username = Username,
                PasswordHash = _passwordHasher.HashPassword(null, Password), // ����������� ������
                Role = "User" // ������ ������ ���� "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index"); // �������������� �� ��������� ��������
        }
    }
}