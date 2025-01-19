using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.EntityFrameworkCore;
namespace AspNetCoreOpeniddictPlus.Web.Persistence;

public class OpeniddictPlusDbContext(DbContextOptions<OpeniddictPlusDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseOpenIddict();
    }
    
    public DbSet<OpeniddictPlusUser> OpeniddictPlusUsers { get; set; }
    public DbSet<OpeniddictPlusRole> OpeniddictPlusRoles { get; set; }
}