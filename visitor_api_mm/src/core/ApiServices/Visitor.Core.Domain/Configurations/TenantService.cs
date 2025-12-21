namespace Visitor.Core.Domain.Configurations;

public class TenantService
{
    private readonly AsyncLocal<string> _tenantId = new();
    public string GetTenantId() => _tenantId.Value!;
    public void SetTenantId(string tenantId) => _tenantId.Value = tenantId;
}