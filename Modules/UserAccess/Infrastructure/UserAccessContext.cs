using BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Modules.UserAccess.Domain.RefreshTokens;
using Modules.UserAccess.Domain.Users;

namespace Modules.UserAccess.Infrastructure;

public class UserAccessContext(DbContextOptions options) : AppDbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            modelBuilder.Entity(entity.Name).ToTable(entity.Name.Split(".").Last(), nameof(UserAccess));
        }

        modelBuilder.Entity<User>()
            .HasMany(e => e.Roles)
            .WithMany();

        modelBuilder.Entity<Role>()
            .HasMany(e => e.Permissions)
            .WithMany();
    }
}