namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityUserLoginConfiguration : BasePostGreSqlConfiguration<IdentityUserLogin>
{
    public override void Configure(EntityTypeBuilder<IdentityUserLogin> entity)
    {
        base.Configure(entity);
        entity.Property(e => e.Login_Source_Sytem)
            .HasMaxLength(MaxLengthValues.LoginSourceSystem)
            .IsUnicode(false);
        entity.Property(e => e.Login_Source_Sytem_Ip)
            .HasMaxLength(MaxLengthValues.LoginSourceSystemIp)
            .IsUnicode(false);
    }
}
