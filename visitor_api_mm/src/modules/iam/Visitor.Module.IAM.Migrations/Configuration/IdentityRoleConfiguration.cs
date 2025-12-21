namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityRoleConfiguration : BasePostGreSqlConfiguration<IdentityRole>
{
    public override void Configure(EntityTypeBuilder<IdentityRole> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.Name)
            .HasMaxLength(MaxLengthValues.RoleName)
            .IsUnicode(false);

        entity.Property(e => e.Tags)
            .HasMaxLength(MaxLengthValues.Tags)
            .IsUnicode(false);
    }
}
