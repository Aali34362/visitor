namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityRolePolicyConfiguration : BasePostGreSqlConfiguration<IdentityRolePolicyMapping>
{
    public override void Configure(EntityTypeBuilder<IdentityRolePolicyMapping> entity)
    {
        base.Configure(entity);

        entity.HasIndex(e => e.policy_Id)
            .HasDatabaseName("ix_rolepolicy_policy_id");

        entity.HasIndex(e => e.role_Id)
              .HasDatabaseName("ix_rolepolicy_role_id");
    }
}