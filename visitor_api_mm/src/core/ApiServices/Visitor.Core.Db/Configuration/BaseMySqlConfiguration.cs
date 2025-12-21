namespace Visitor.Core.Db.Configuration;

public abstract class BaseMySqlConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseModel
{
    public virtual void Configure(EntityTypeBuilder<T> entity)
    {
        //Fluent Api configurations
        entity.ToTable($"{typeof(T).Name.ToLower()}");

        entity.HasKey(e => e.Id).HasName($"pk_{typeof(T).Name.ToLower()}");

        entity.Property(e => e.CreatedAt).HasColumnType("DATETIME");
        entity.Property(e => e.UpdatedAt).HasColumnType("DATETIME");

        entity.Property(e => e.CreatedBy)
           .HasMaxLength(150)
           .IsUnicode(false);

        entity.Property(e => e.UpdatedBy)
          .HasMaxLength(150)
          .IsUnicode(false);

        entity.HasKey(e => e.Id).HasName($"pk_{typeof(T).Name.ToLower()}");

        entity.HasIndex(nameof(BaseModel.IsDeleted))
         .HasDatabaseName($"ix_{typeof(T).Name.ToLower()}_isdeleted");
    }
}
