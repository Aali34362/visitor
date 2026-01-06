namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityModuleConfiguration : BasePostGreSqlConfiguration<IdentityModule>
{
    public override void Configure(EntityTypeBuilder<IdentityModule> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.name)
             .HasMaxLength(MaxLengthValues.ModuleName)
            .IsUnicode(false);

        entity.Property(e => e.tags)
            .HasMaxLength(MaxLengthValues.Tags)
           .IsUnicode(false);
    }
}
