using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Core.ViewModels;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration.Helpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration.User;


public class Index(IUserService<OpeniddictPlusUser> userService,
    UserManager<OpeniddictPlusUser> userManager) : PageModel
{
    
    
    public PagedResult<OpeniddictPlusUser>? Users { get; set; }
    public List<ColumnDefinition<OpeniddictPlusUser>> Columns { get; set; } =
    [
        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Name",
            Template = item => new HtmlString($"{item.FirstName} {item.LastName}"),
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
                $"<a  data-user-id={item.Id} class=\"font-medium cursor-pointer text-blue-600 dark:text-blue-500 hover:underline\">Edit</a>\n <a href=\"#\" class=\"font-medium text-red-600 dark:text-red-500 hover:underline\">Delete</a>\n"
            ),
        }

    ];
    
    [BindProperty]
    public UserViewModel User { get; set; }
    
    [TempData]
    public string ErrorMessage { get; set; } = string.Empty;
    
    [FromQuery(Name = "userId")]
    public string? UserId { get; set; } = null;
    
    public bool preserveDialogForm { get; set; } = false;
    
    public PaginationViewModel Pagination { get; set; }
    
    public string StatusMessage { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(string? userId = null)
    {
        User = new UserViewModel();
        if (UserId is not null)
        {
          
            var user = await userManager.FindByIdAsync(UserId);
            if (user is not null)
            {
                User.UserName = user.UserName;
                User.Email = user.Email;
                User.FirstName = user.FirstName;
                User.LastName = user.LastName;
            }
        }
        
        Users = await userService.GetUsersAsync();
        Pagination = new PaginationViewModel
        {
            PageSize = Users.PageSize,
            CurrentPage = Users.CurrentPage,
            TotalPages = Users.TotalCount,
            HasNextPage = Users.HasNextPage,
            HasPreviousPage = Users.HasPreviousPage
        };
        return Page();
    }

    public IActionResult RedirectToEdit()
    {
        return RedirectToPage("/Identity/Administration/UserManagement",new { userId = UserId });
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            preserveDialogForm = true;
            ErrorMessage = string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
            return await OnGetAsync();

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
                return RedirectToPage();
            }
            ErrorMessage = "Error creating user";
            return Page();

        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            preserveDialogForm = true;
            await OnGetAsync();
            return Page();
        }
    }
}
