namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityPolicyPageActionConfiguration : BasePostGreSqlConfiguration<IdentityPolicyPageActionMapping>
{
    public override void Configure(EntityTypeBuilder<IdentityPolicyPageActionMapping> entity)
    {
        base.Configure(entity);
        entity.HasIndex(e => e.Policy_Id)
             .HasDatabaseName("ix_policypageactions_policy_id");

        entity.HasIndex(e => e.PageAction_Id)
              .HasDatabaseName("ix_policypageactions_pageaction_id");
    }
}
