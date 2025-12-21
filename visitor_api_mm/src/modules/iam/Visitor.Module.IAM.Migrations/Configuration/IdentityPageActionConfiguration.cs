namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityPageActionConfiguration : BasePostGreSqlConfiguration<IdentityPageAction>
{
    public override void Configure(EntityTypeBuilder<IdentityPageAction> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.Name)
             .HasMaxLength(MaxLengthValues.PageActionName)
            .IsUnicode(false);

        entity.Property(e => e.Action)
            .HasMaxLength(MaxLengthValues.PageActionAction)
           .IsUnicode(false);

        entity.Property(e => e.AccessLevel)
           .HasMaxLength(MaxLengthValues.PageActionAccessLevel)
          .IsUnicode(false);

        entity.Property(e => e.PageUrl)
           .HasMaxLength(MaxLengthValues.PageActionUrl)
          .IsUnicode(false);
    }
}