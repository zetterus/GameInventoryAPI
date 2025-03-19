using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameInventoryAPI.Pages
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public IActionResult OnPost()
        {
            // ������� JWT �� cookies
            Response.Cookies.Delete("jwt", new CookieOptions
            {
                Path = "/", // ��������� ����, ��� �������� ��������� ����
                Secure = true, // ������ ��� HTTPS
                SameSite = SameSiteMode.Strict // ������ �� CSRF
            });

            // �������������� �� ������� ��������
            return RedirectToPage("/Index");
        }
    }
}