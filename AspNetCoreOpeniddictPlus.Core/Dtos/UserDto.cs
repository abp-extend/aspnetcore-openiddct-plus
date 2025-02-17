namespace AspNetCoreOpeniddictPlus.Core.Dtos;

public sealed class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public bool EmailConfirmed { get; set; } = false; 
    public bool CreatedByAdmin { get; set; } = false;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<string> Roles { get; set; } = [];
    public DateTime? DeletionRequestedAt { get; set; }
}