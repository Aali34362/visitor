namespace Visitor.Core.Db.Configuration;

public abstract class BasePostGreSqlConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseModel
{
    public virtual void Configure(EntityTypeBuilder<T> entity)
    {
        //Fluent Api configurations
        ////string entityName = typeof(T).Name.ToLower();
        ////var schema = GetModuleSchemaName(typeof(T));
        ////entity.ToTable(entityName, schema: schema);
        var dbSetName = entity.Metadata.GetDefaultTableName()?.ToLower();

        // Fallback to CLR type name
        var tableName = dbSetName ?? typeof(T).Name.ToLower();

        entity.ToTable(tableName);

        entity.HasKey(e => e.id).HasName($"pk_{typeof(T).Name.ToLower()}");

        entity.Property(e => e.created_At).HasColumnType("timestamp without time zone");
        entity.Property(e => e.updated_At).HasColumnType("timestamp without time zone");

        entity.Property(e => e.created_By)
           .HasMaxLength(150)
           .IsUnicode(false);

        entity.Property(e => e.updated_By)
          .HasMaxLength(150)
          .IsUnicode(false);

        entity.HasKey(e => e.id).HasName($"pk_{tableName}");

        entity.HasIndex(nameof(BaseModel.is_Deleted))
         .HasDatabaseName($"ix_{tableName}_isdeleted");

        entity.HasIndex(e => new { e.id, e.is_Deleted })
          .HasDatabaseName($"ix_{tableName}_id_isdeleted");
    }

    private string GetModuleSchemaName(Type type)
    {
        // Example namespace: Inventory.Core.DomainEFWork.Data.Configuration.IAM
        var namespaceParts = type.Namespace?.Split('.') ?? Array.Empty<string>();

        // Assume the last segment of the Configuration namespace is the module (e.g., "IAM")
        var schema = namespaceParts.LastOrDefault()?.ToLower() ?? "public";

        return schema;
    }
}