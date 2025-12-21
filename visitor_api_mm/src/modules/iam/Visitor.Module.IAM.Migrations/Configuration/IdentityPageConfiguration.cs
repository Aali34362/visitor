namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityPageConfiguration : BasePostGreSqlConfiguration<IdentityPage>
{
    public override void Configure(EntityTypeBuilder<IdentityPage> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.Page_Title)
             .HasMaxLength(MaxLengthValues.PageTitle)
            .IsUnicode(false);

        entity.Property(e => e.Page_Url)
            .HasMaxLength(MaxLengthValues.PageUrl)
           .IsUnicode(false);

        entity.Property(e => e.Page_Nm)
           .HasMaxLength(MaxLengthValues.PageName)
          .IsUnicode(false);

        entity.Property(e => e.Icon)
           .HasMaxLength(MaxLengthValues.Icon)
          .IsUnicode(false);
    }
}