using API.Source.Db.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Source.Db;

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) { }

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
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTimeOffset.UtcNow,
                    UpdatedTime = DateTimeOffset.UtcNow,
                    Price = 0,
                    Type = TenantConfiguration.TenantType.Free,
                },
                new TenantConfiguration
                {
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTimeOffset.UtcNow,
                    UpdatedTime = DateTimeOffset.UtcNow,
                    Price = new decimal(9.99),
                    Type = TenantConfiguration.TenantType.Basic,
                },
                new TenantConfiguration
                {
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTimeOffset.UtcNow,
                    UpdatedTime = DateTimeOffset.UtcNow,
                    Price = new decimal(19.99),
                    Type = TenantConfiguration.TenantType.Premium,
                }
            );
        });
    }
}
