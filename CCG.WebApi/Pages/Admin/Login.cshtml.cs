using System.ComponentModel.DataAnnotations;
using CCG.Application;
using CCG.Application.Contracts.Services.Identity;
using CCG.Application.Utilities;
using CCG.Domain.Entities.Identity;
using CCG.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CCG.WebApi.Pages.Admin
{
	[AllowAnonymous]
    public class LoginModel(
		    SignInManager<UserEntity> signInManager,
			ILogger<LoginModel> logger,
			UserManager<UserEntity> userManager, 
			IIdentityProviderService identityProvider, 
			RoleManager<IdentityRole> roleManager
		)
		: PageModel
	{

		[BindProperty]
		public InputModel Input { get; set; }

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public string ReturnUrl { get; set; }

		[TempData]
		public string ErrorMessage { get; set; }

		public class InputModel
		{
			[Required]
			public string UserName { get; set; }

			[Required]
			[DataType(DataType.Password)]
			public string Password { get; set; }

			[Display(Name = "Remember me?")]
			public bool RememberMe { get; set; }
		}

		public async Task OnGetAsync(string returnUrl = null)
		{
			if (!string.IsNullOrEmpty(ErrorMessage))
			{
				ModelState.AddModelError(string.Empty, ErrorMessage);
			}

			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			// ReturnUrl = Url.Content("~/admin/menu");
			ReturnUrl = Url.Content("~/swagger/index");
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			// returnUrl ??= Url.Content("~/admin/menu");
			returnUrl ??= Url.Content("~/swagger/index");

			if (!ModelState.IsValid)
				return Page();

			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, set lockoutOnFailure: true
			var result = await signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
			if (result.Succeeded)
			{
				var user = await userManager.FindByNameAsync(Input.UserName);
				user = await identityProvider.UpdateTokenAsync(user);

				Response.Cookies.Append(Constants.AccessTokenParam, user.AccessToken);

				logger.LogInformation("User logged in.");
				return LocalRedirect(returnUrl);
			}
			if (result.RequiresTwoFactor)
			{
				return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
			}
			if (result.IsLockedOut)
			{
				logger.LogWarning("User account locked out.");
				return RedirectToPage("./Lockout");
			}

			ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			return Page();
		}
    }
}