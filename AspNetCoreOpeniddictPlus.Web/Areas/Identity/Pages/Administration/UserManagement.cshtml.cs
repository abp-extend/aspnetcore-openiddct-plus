using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Core.ViewModels;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration.Helpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration;


public class UserManagement(IUserService<OpeniddictPlusUser> userService,
    UserManager<OpeniddictPlusUser> userManager) : PageModel
{
    
    
    public PagedResult<OpeniddictPlusUser>? Users { get; set; }
    public List<ColumnDefinition<OpeniddictPlusUser>> Columns { get; set; } =
    [
        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Name",
            Template = item => new HtmlString("N/A"),
        },

        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "User name",
            Template = item => new HtmlString(item.UserName ?? "N/A"),
        },

        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Email",
            Template = item => new HtmlString(item.Email ?? "N/A"),
        },

        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Phone number",
            Template = item => new HtmlString(item.PhoneNumber ?? "N/A"),
        },

        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Lockout enabled",
            Template = item => new HtmlString(item.LockoutEnabled ? "Disabled" : "Enabled"),
        },

        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Email confirmed",
            Template = item => new HtmlString(item.EmailConfirmed ? "Yes" : "No"),
        },

        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Action",
            Template = item => new HtmlString(
                $"<a href=\"#\" class=\"font-medium text-blue-600 dark:text-blue-500 hover:underline\">Edit user</a>\n"
            ),
        }

    ];
    
    [BindProperty]
    public UserViewModel User { get; set; }
    
    [TempData]
    public string ErrorMessage { get; set; } = string.Empty;
    
    public string StatusMessage { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        User = new UserViewModel();
        Users = await userService.GetUsersAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            await OnGetAsync();
            return Page();
        }
        
        try
        {
            var user = new OpeniddictPlusUser
            {
                UserName = User.UserName,
                Email = User.Email
            };
            
            var result = await userManager.CreateAsync(user, User.Password);
            if (result.Succeeded) return RedirectToPage();
            StatusMessage = "Error creating user";
            return Page();

        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            await OnGetAsync();
            return Page();
        }
    }
}
