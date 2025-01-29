using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Core.ViewModels;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration.Role;

public class Create(IRoleService<OpeniddictPlusRole> roleService) : PageModel
{
    [BindProperty]
    public RoleViewModel Role { get; set; }
        
    [TempData]
    public string ErrorMessage { get; set; } = string.Empty;

    public string StatusMessage { get; set; } = string.Empty;
    
    public IActionResult OnGet()
    {
        Role = new RoleViewModel();
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
            var role = new OpeniddictPlusRole { Name = Role.Name };
            await roleService.CreateRoleAsync(role);
            StatusMessage = "Role created successfully.";
            return RedirectToPage("./Index", new {});
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while saving the role.");
           return OnGet();
        }
    }
}