namespace Visitor.Module.DMS.Migrations.Configuration;

public class DocumentConfiguration : BasePostGreSqlConfiguration<Document>
{
    public override void Configure(EntityTypeBuilder<Document> entity)
    {
        base.Configure(entity);

        

        entity.Property(e => e.Tags)
            .HasMaxLength(MaxLengthValues.Tags)
           .IsUnicode(false);
    }
}