namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityPageActionConfiguration : BasePostGreSqlConfiguration<IdentityPageAction>
{
    public override void Configure(EntityTypeBuilder<IdentityPageAction> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.name)
             .HasMaxLength(MaxLengthValues.PageActionName)
            .IsUnicode(false);

        entity.Property(e => e.action)
            .HasMaxLength(MaxLengthValues.PageActionAction)
           .IsUnicode(false);

        entity.Property(e => e.access_Level)
           .HasMaxLength(MaxLengthValues.PageActionAccessLevel)
          .IsUnicode(false);

        entity.Property(e => e.page_Url)
           .HasMaxLength(MaxLengthValues.PageActionUrl)
          .IsUnicode(false);
    }
}