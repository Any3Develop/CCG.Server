using CCG.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CCG.WebApi.Pages.Admin
{
    [AllowAnonymous]
    public class LogoutModel(SignInManager<UserEntity> signInManager, ILogger<LogoutModel> logger) : PageModel
    {
	    public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            logger.LogInformation("User logged out.");
            returnUrl = Url.Content("~/admin/login");
            return LocalRedirect(returnUrl);
        }
    }
}
