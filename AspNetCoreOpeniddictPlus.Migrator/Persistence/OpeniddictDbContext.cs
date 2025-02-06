using AspNetCoreOpeniddictPlus.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreOpeniddictPlus.Migrator.Persistence;

public class OpeniddictPlusDbContext(DbContextOptions<OpeniddictPlusDbContext> options)
    : IdentityDbContext(options)
{
    public DbSet<OpeniddictPlusUser> OpeniddictPlusUsers { get; set; }
    public DbSet<OpeniddictPlusRole> OpeniddictPlusRoles { get; set; }
    public DbSet<OpeniddictPlusPermission> OpeniddictPlusPermissions { get; set; }
    public DbSet<OpeniddictPlusRolePermission> OpeniddictPlusRolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseOpenIddict();

        modelBuilder.Entity<OpeniddictPlusPermission>(entity =>
        {
            entity.ToTable("OpeniddictPlusPermissions");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).HasMaxLength(50).IsRequired();
            entity.Property(p => p.Description).HasMaxLength(200);
        });

        modelBuilder.Entity<OpeniddictPlusRole>(entity =>
        {
            entity.ToTable("OpeniddictPlusRoles");
            entity.Property(r => r.Name).HasMaxLength(50).IsRequired();
        });

        // Configure Many-to-Many Relationship
        modelBuilder.Entity<OpeniddictPlusRolePermission>()
            .ToTable("OpeniddictPlusRolePermissions")
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<OpeniddictPlusRolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        modelBuilder.Entity<OpeniddictPlusRolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);
    }
}