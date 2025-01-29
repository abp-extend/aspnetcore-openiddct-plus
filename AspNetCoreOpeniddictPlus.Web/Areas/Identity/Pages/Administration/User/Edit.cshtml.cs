using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Core.ViewModels;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration.User;

public class Edit(IUserService<OpeniddictPlusUser> userService,
    UserManager<OpeniddictPlusUser> userManager, ILogger<Edit> logger) : PageModel
{
    [BindProperty]
    public UserViewModel User { get; set; }
    
    [TempData]
    public string ErrorMessage { get; set; } = string.Empty;
    
    public string StatusMessage { get; set; } = string.Empty;
    
    [FromQuery(Name = "userId")]
    public string? UserId { get; set; } = null;
    
    public async Task<IActionResult> OnGetAsync()
    {
        if (string.IsNullOrEmpty(UserId))
        {
            ErrorMessage = "User id is required";
            logger.LogError($"User id is not provided");
            return RedirectToPage("/Error");
        }

        try
        {
            var user = await userService.GetUserByIdAsync(UserId);
            User = new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
            };

            return Page();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting user");
            return RedirectToPage("/Error");
        }
    }
}