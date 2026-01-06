namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityPageConfiguration : BasePostGreSqlConfiguration<IdentityPage>
{
    public override void Configure(EntityTypeBuilder<IdentityPage> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.page_Title)
             .HasMaxLength(MaxLengthValues.PageTitle)
            .IsUnicode(false);

        entity.Property(e => e.page_Url)
            .HasMaxLength(MaxLengthValues.PageUrl)
           .IsUnicode(false);

        entity.Property(e => e.page_Nm)
           .HasMaxLength(MaxLengthValues.PageName)
          .IsUnicode(false);

        entity.Property(e => e.icon)
           .HasMaxLength(MaxLengthValues.Icon)
          .IsUnicode(false);
    }
}