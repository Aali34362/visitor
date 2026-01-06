namespace Visitor.Core.Db.Configuration;

public abstract class BaseMySqlConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseModel
{
    public virtual void Configure(EntityTypeBuilder<T> entity)
    {
        //Fluent Api configurations
        entity.ToTable($"{typeof(T).Name.ToLower()}");

        entity.HasKey(e => e.id).HasName($"pk_{typeof(T).Name.ToLower()}");

        entity.Property(e => e.created_At).HasColumnType("DATETIME");
        entity.Property(e => e.updated_At).HasColumnType("DATETIME");

        entity.Property(e => e.created_By)
           .HasMaxLength(150)
           .IsUnicode(false);

        entity.Property(e => e.updated_By)
          .HasMaxLength(150)
          .IsUnicode(false);

        entity.HasKey(e => e.id).HasName($"pk_{typeof(T).Name.ToLower()}");

        entity.HasIndex(nameof(BaseModel.is_Deleted))
         .HasDatabaseName($"ix_{typeof(T).Name.ToLower()}_isdeleted");
    }
}
