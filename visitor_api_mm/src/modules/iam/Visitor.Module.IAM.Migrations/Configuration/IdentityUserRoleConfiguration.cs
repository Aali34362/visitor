namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityUserRoleConfiguration : BasePostGreSqlConfiguration<IdentityUserRoleMapping>
{
    public override void Configure(EntityTypeBuilder<IdentityUserRoleMapping> entity)
    {
        base.Configure(entity);

        entity.HasIndex(e => e.user_Id)
           .HasDatabaseName("ix_userrole_user_id");

        entity.HasIndex(e => e.role_Id)
              .HasDatabaseName("ix_userrole_role_id");
    }
}
