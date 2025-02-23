using API.Source.Db.Models;
using API.Source.Guards;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Source.Db;

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<TenantConfiguration> TenantConfigurations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        builder.Entity<TenantConfiguration>(config =>
        {
            config.Property(t => t.CreatedTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            config.Property(t => t.UpdatedTime).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            config.Property(t => t.Price).HasColumnType("decimal(18, 4)");
            config.HasData(
                new TenantConfiguration
                {
                    Id = 1_000_002,
                    CreatedTime = DateTimeOffset.UtcNow,
                    UpdatedTime = DateTimeOffset.UtcNow,
                    Price = 0,
                    Type = TenantConfiguration.TENANT_TYPE.Free,
                },
                new TenantConfiguration
                {
                    Id = 1_000_001,
                    CreatedTime = DateTimeOffset.UtcNow,
                    UpdatedTime = DateTimeOffset.UtcNow,
                    Price = new decimal(9.99),
                    Type = TenantConfiguration.TENANT_TYPE.Basic,
                },
                new TenantConfiguration
                {
                    Id = 1_000_000,
                    CreatedTime = DateTimeOffset.UtcNow,
                    UpdatedTime = DateTimeOffset.UtcNow,
                    Price = new decimal(19.99),
                    Type = TenantConfiguration.TENANT_TYPE.Premium,
                }
            );
        });

        const string adminRoleId = "1175d6a8-75f1-4f79-b509-b85bcdcd5eaa";
        const string elevatedRoleId = "ca897b38-a584-4767-8b01-89ffaacf5bfa";
        const string moderatorRoleId = "f3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b";
        const string noneRoleId = "a4f720c0-0f99-4c71-8d8e-797a07f983f4";
        const string adminUserId = "bb656271-845e-43ac-ae82-5f95db9eec0a";

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = adminRoleId, Name = Policies.Admin, NormalizedName = Policies.Admin.ToUpper() },
            new IdentityRole { Id = elevatedRoleId, Name = Policies.Elevated, NormalizedName = Policies.Elevated.ToUpper() },
            new IdentityRole { Id = moderatorRoleId, Name = Policies.Moderator, NormalizedName = Policies.Moderator.ToUpper() },
            new IdentityRole { Id = noneRoleId, Name = Policies.None, NormalizedName = Policies.None.ToUpper() }
        );

        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(new ApplicationUser(), "Admin123"),
                SecurityStamp = string.Empty,
            });

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { RoleId = adminRoleId, UserId = adminUserId }
        );
    }

}