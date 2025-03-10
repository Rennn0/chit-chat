﻿using API.Source.Db.Models;

namespace API.Source;

public class ResponseModelBase<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public object? Error { get; set; } = null;
}

public class AddNewTenantResponse
{
    public TenantConfiguration? Configuration { get; set; }
}

public class ListTenantsResponse
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public TenantConfiguration.TENANT_TYPE Type { get; set; }
}
