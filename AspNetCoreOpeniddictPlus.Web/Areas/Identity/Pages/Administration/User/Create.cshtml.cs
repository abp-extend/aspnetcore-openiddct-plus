using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Core.ViewModels;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration.User;

public class Create(IUserService<OpeniddictPlusUser> userService,
    UserManager<OpeniddictPlusUser> userManager) : PageModel
{
    [BindProperty]
    public UserViewModel User { get; set; }
    
    [TempData]
    public string ErrorMessage { get; set; } = string.Empty;
    
    public string StatusMessage { get; set; } = string.Empty;

    
    public IActionResult OnGet()
    {
        User = new UserViewModel();
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            return OnGet();

        }
        
        try
        {
            var user = new OpeniddictPlusUser
            {
                UserName = User.UserName,
                Email = User.Email,
                CreatedByAdmin = true,
                FirstName = User.FirstName,
                LastName = User.LastName,
                PasswordChangeRequired = true
            };
            
            var result = await userManager.CreateAsync(user, User.Password);
            if (result.Succeeded)
            {
                StatusMessage = "Successfully created user";
                return RedirectToPage("./Index", new {});
            }
            ErrorMessage = "Error creating user";
            return Page();

        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            return Page();
        }
    }
}