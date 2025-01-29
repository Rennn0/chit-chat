namespace CC.Shop.Library.Source;

public class FreeTenantFactory : ITenantFactory
{
    public ITenantEntity CreateTenant()
    {
        return new FreeTenantEntity();
    }

    public Task<ITenantEntity> CreateTenantAsync()
    {
        throw new NotImplementedException();
    }
}

public class BasicTenantFactory : ITenantFactory
{
    public ITenantEntity CreateTenant()
    {
        return new BasicTenantEntity();
    }

    public Task<ITenantEntity> CreateTenantAsync()
    {
        throw new NotImplementedException();
    }
}

public class PremiumTenantFactory : ITenantFactory
{
    public ITenantEntity CreateTenant()
    {
        return new PremiumTenantEntity();
    }

    public Task<ITenantEntity> CreateTenantAsync()
    {
        throw new NotImplementedException();
    }
}
