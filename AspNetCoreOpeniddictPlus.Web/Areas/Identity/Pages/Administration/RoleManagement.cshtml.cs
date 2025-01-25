using System.Linq;
using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration.Helpers;
using AspNetCoreOpeniddictPlus.Web.Persistence;
using AspNetCoreOpeniddictPlus.Web.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration
{
    public class RoleManagementModel(IRoleService<OpeniddictPlusRole> roleService) : PageModel
    {

        public PagedResult<OpeniddictPlusRole>? Roles { get; set; }
        
        public List<ColumnDefinition<OpeniddictPlusRole>> Columns { get; set; } =
        [
            new ColumnDefinition<OpeniddictPlusRole>
            {
                Header = "Name",
                Template = item => new HtmlString(item.Name),
            },

            new ColumnDefinition<OpeniddictPlusRole>
            {
                Header = "Action",
                Template = item => new HtmlString(
                    $"<a href=\"#\" class=\"font-medium text-blue-600 dark:text-blue-500 hover:underline\">Edit role</a>\n"
                ),
            }

        ];
        
        [BindProperty]
        public RoleViewModel Role { get; set; }
        
        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;
        
        public string StatusMessage { get; set; } = string.Empty;
     

        public async Task OnGetAsync()
        {
            Role = new RoleViewModel();
            Roles = await roleService.GetRolesAsync();
        }

        public async Task<IActionResult> OnGetEditAsync(string id)
        {
            var role = await roleService.GetRoleByIdAsync(id);
            Role = new RoleViewModel { Id = role.Id, Name = role.Name  ?? "N/A"};
            await OnGetAsync();
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
                if (string.IsNullOrEmpty(Role.Id))
                {
                    var role = new OpeniddictPlusRole { Name = Role.Name };
                    await roleService.CreateRoleAsync(role);
                    StatusMessage = "Role created successfully.";
                }
                else
                {
                    try
                    {
                        var existingRole = await roleService.GetRoleByIdAsync(Role.Id);
                        existingRole.Name = Role.Name;
                        StatusMessage = "Role updated successfully.";
                    }
                    catch (Exception e)
                    {
                        return NotFound(e.Message);
                    }
                    
                }
                
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the role.");
                await OnGetAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            try
            {
                await roleService.DeleteRoleAsync(id);
                StatusMessage = "Role deleted successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = "Error: Unable to delete role.";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync(string id)
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
                var role = await roleService.GetRoleByIdAsync(id);
                role.Name = Role.Name;
                await roleService.UpdateRoleAsync(id, role);
            }
            catch (Exception ex)
            {
                StatusMessage = "Error: Unable to update role.";
            }
            return RedirectToPage();
        }
    }
}
