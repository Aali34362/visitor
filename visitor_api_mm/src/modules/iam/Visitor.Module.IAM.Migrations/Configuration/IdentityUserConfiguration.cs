namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityUserConfiguration : BasePostGreSqlConfiguration<IdentityUser>
{
    public override void Configure(EntityTypeBuilder<IdentityUser> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.UserName)
            .HasMaxLength(MaxLengthValues.UserName)
            .IsUnicode(false);

        entity.Property(e => e.FirstName)
            .HasMaxLength(MaxLengthValues.FirstName)
            .IsUnicode(false);

        entity.Property(e => e.LastName)
            .HasMaxLength(MaxLengthValues.LastName)
            .IsUnicode(false);

        entity.Property(e => e.Email)
            .HasMaxLength(MaxLengthValues.Email)
            .IsUnicode(false);

        entity.Property(e => e.PhoneNumber)
            .HasMaxLength(MaxLengthValues.PhoneNumber)
            .IsUnicode(false);

        entity.Property(e => e.PasswordHash)
            .HasMaxLength(MaxLengthValues.PasswordHash)
            .IsUnicode(false);
    }
}
