namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityModuleConfiguration : BasePostGreSqlConfiguration<IdentityModule>
{
    public override void Configure(EntityTypeBuilder<IdentityModule> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.Name)
             .HasMaxLength(MaxLengthValues.ModuleName)
            .IsUnicode(false);

        entity.Property(e => e.Tags)
            .HasMaxLength(MaxLengthValues.Tags)
           .IsUnicode(false);
    }
}
