using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Web.Persistence;

public class OpeniddictPlusDbContext(DbContextOptions<OpeniddictPlusDbContext> options) : IdentityDbContext(options)
{
    public DbSet<OpeniddictPlusUser> OpeniddictPlusUsers { get; set; }
    public DbSet<OpeniddictPlusRole> OpeniddictPlusRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseOpenIddict();
    }
}