using System.Linq;
using AspNetCoreOpeniddictPlus.Identity.Entities;
using AspNetCoreOpeniddictPlus.Web.Persistence;
using AspNetCoreOpeniddictPlus.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Web.Areas.Identity.Pages.Administration
{
    public class RoleManagementModel : PageModel
    {
        private readonly OpeniddictPlusDbContext _context;

        public RoleManagementModel(OpeniddictPlusDbContext context)
        {
            _context = context;
            Roles = new List<RoleViewModel>();
            Role = new RoleViewModel();
        }

        [BindProperty]
        public IList<RoleViewModel> Roles { get; set; }

        [BindProperty]
        public RoleViewModel Role { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            Roles = await _context
                .Roles.Select(r => new RoleViewModel { Id = r.Id, Name = r.Name })
                .ToListAsync();
        }

        public async Task<IActionResult> OnGetEditAsync(string id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            Role = new RoleViewModel { Id = role.Id, Name = role.Name };
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
                    _context.Roles.Add(role);
                    StatusMessage = "Role created successfully.";
                }
                else
                {
                    var existingRole = await _context.Roles.FindAsync(Role.Id);
                    if (existingRole == null)
                    {
                        return NotFound();
                    }
                    existingRole.Name = Role.Name;
                    StatusMessage = "Role updated successfully.";
                }
                
                await _context.SaveChangesAsync();
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
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            try
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                StatusMessage = "Role deleted successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = "Error: Unable to delete role.";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                await OnGetAsync();
                return Page();
            }

            var role = await _context.Roles.FindAsync(Role.Id);
            if (role != null)
            {
                role.Name = Role.Name;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
