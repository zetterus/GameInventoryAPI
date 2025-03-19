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
            // Удаляем JWT из cookies
            Response.Cookies.Delete("jwt", new CookieOptions
            {
                Path = "/", // Указываем путь, для которого действует кука
                Secure = true, // Только для HTTPS
                SameSite = SameSiteMode.Strict // Защита от CSRF
            });

            // Перенаправляем на главную страницу
            return RedirectToPage("/Index");
        }
    }
}