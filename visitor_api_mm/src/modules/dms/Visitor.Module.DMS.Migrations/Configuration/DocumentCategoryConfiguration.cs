namespace Visitor.Module.DMS.Migrations.Configuration;

internal class DocumentCategoryConfiguration : BasePostGreSqlConfiguration<DocumentCategory>
{
    public override void Configure(EntityTypeBuilder<DocumentCategory> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.Name)
            .HasMaxLength(MaxLengthValues.DocumentCategoryName)
           .IsUnicode(false);

        entity.Property(e => e.Tags)
            .HasMaxLength(MaxLengthValues.Tags)
           .IsUnicode(false);
    }
}