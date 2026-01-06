namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityUserConfiguration : BasePostGreSqlConfiguration<IdentityUser>
{
    public override void Configure(EntityTypeBuilder<IdentityUser> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.user_Nm)
            .HasMaxLength(MaxLengthValues.UserName)
            .IsUnicode(false);

        entity.Property(e => e.first_Nm)
            .HasMaxLength(MaxLengthValues.FirstName)
            .IsUnicode(false);

        entity.Property(e => e.last_Nm)
            .HasMaxLength(MaxLengthValues.LastName)
            .IsUnicode(false);

        entity.Property(e => e.email)
            .HasMaxLength(MaxLengthValues.Email)
            .IsUnicode(false);

        entity.Property(e => e.phone_No)
            .HasMaxLength(MaxLengthValues.PhoneNumber)
            .IsUnicode(false);

        entity.Property(e => e.password_Hash)
            .HasMaxLength(MaxLengthValues.PasswordHash)
            .IsUnicode(false);
    }
}
