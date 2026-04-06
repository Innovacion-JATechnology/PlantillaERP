using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserRoles.Identity.Models;

namespace UserRoles.Identity.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }

        // DbSets para los modelos de permisos
        public DbSet<ModulePermission> ModulePermissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relación entre RolePermission y IdentityRole
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany()
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar relación entre RolePermission y ModulePermission
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.ModulePermission)
                .WithMany()
                .HasForeignKey(rp => rp.ModulePermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

