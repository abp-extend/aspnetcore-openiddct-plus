using AspNetCoreOpeniddictPlus.Core.Dtos;
using AspNetCoreOpeniddictPlus.Core.Interfaces;
using AspNetCoreOpeniddictPlus.Core.ViewModels;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration.Helpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration.Role
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
                    $"<span data-edit-user={item.Id} class=\"font-medium cursor-pointer text-blue-600 dark:text-blue-500 hover:underline\">Edit</span>\n <span  data-delete-user-id={item.Id} class=\"font-medium  cursor-pointer text-red-600 dark:text-red-500 hover:underline\">Delete</span>\n"
                ),
            }

        ];
        
        [BindProperty]
        public RoleViewModel Role { get; set; }
        
        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;
        
        public bool preserveDialogForm { get; set; } = false;
        
        public PaginationViewModel Pagination { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
     

        public async Task OnGetAsync()
        {
            Role = new RoleViewModel();
            Roles = await roleService.GetRolesAsync();
            Pagination = new PaginationViewModel
            {
                PageSize = Roles.PageSize,
                CurrentPage = Roles.CurrentPage,
                TotalPages = Roles.TotalCount,
                HasNextPage = Roles.HasNextPage,
                HasPreviousPage = Roles.HasPreviousPage
            };
        }

        public async Task<IActionResult> OnGetEditAsync(string id)
        {
            var role = await roleService.GetRoleByIdAsync(id);
            Role = new RoleViewModel { Id = role.Id, Name = role.Name  ?? "N/A"};
            await OnGetAsync();
            return Page();
        }
        

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            try
            {
                await roleService.DeleteRoleAsync(id);
                StatusMessage = "Successfully deleted role.";
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
                preserveDialogForm = true;
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
                StatusMessage = "Successfully updated role.";

            }
            catch (Exception ex)
            {
                StatusMessage = "Error: Unable to update role.";
            }
            return RedirectToPage();
        }
    }
}
