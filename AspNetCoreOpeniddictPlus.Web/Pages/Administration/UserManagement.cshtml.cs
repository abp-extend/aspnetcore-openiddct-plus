using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreOpeniddictPlus.Web.Pages.Administration;

public class ColumnDefinition<TItem> 
{
  public string Header { get; set; }
  public Func<TItem, IHtmlContent> Template { get; set; }
  
}
public class UserManagement(IUserService<OpeniddictPlusUser> userService) : PageModel
{ 
    public PagedResult<OpeniddictPlusUser>? Users { get; set; }
    public List<ColumnDefinition<OpeniddictPlusUser>> Columns { get; set; } = new List<ColumnDefinition<OpeniddictPlusUser>>
    {
        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Name",
            Template = item => new HtmlString("N/A")
        },
        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "User name",
            Template = item => new HtmlString(item.UserName ?? "N/A")
        },
        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Email",
            Template = item => new HtmlString(item.Email ?? "N/A")
        },
        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Phone number",
            Template = item => new HtmlString(item.PhoneNumber ?? "N/A")
        },
        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Lockout enabled",
            Template = item => new HtmlString(item.LockoutEnabled ? "Disabled" : "Enabled")
        },
        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Email confirmed",
            Template = item => new HtmlString(item.EmailConfirmed ? "Yes" : "No")
        },
        new ColumnDefinition<OpeniddictPlusUser>
        {
            Header = "Action",
            Template = item => new HtmlString($"<a href=\"#\" class=\"font-medium text-blue-600 dark:text-blue-500 hover:underline\">Edit user</a>\n")
        }
    };
    
 

    public async Task<IActionResult> OnGet()
    {
        Users = await userService.GetUsersAsync();
        return Page();
    }
}