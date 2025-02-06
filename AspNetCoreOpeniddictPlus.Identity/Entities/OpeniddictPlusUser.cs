using Microsoft.AspNetCore.Identity;

namespace AspNetCoreOpeniddictPlus.Identity.Entities;

public class OpeniddictPlusUser : IdentityUser
{
    public bool CreatedByAdmin { get; set; } = false;
    public bool PasswordChangeRequired { get; set; } = false;
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public DateTime? DeletionRequestedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }

}
