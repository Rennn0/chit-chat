namespace CC.Shop.Library.Source;

public interface ITenantFactory
{
    ITenantEntity CreateTenant();
    Task<ITenantEntity> CreateTenantAsync();
}

public interface ITenantConfigurationFactory
{
    ITenantConfiguration CreateTenantConfiguration();
    Task<ITenantConfiguration> CreateTenantConfigurationAsync();
}

public interface ITenantEntity { }

public interface ITenantConfiguration { }
