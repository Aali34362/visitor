namespace Visitor.Module.DMS.Migrations.Configuration;

internal class DocumentTypeConfiguration : BasePostGreSqlConfiguration<DocumentType>
{
    public override void Configure(EntityTypeBuilder<DocumentType> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.Name)
            .HasMaxLength(MaxLengthValues.DocumentTypeName)
           .IsUnicode(false);

        entity.Property(e => e.Tags)
            .HasMaxLength(MaxLengthValues.Tags)
           .IsUnicode(false);
    }
}