namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityPolicyConfiguration : BasePostGreSqlConfiguration<IdentityPolicy>
{
    public override void Configure(EntityTypeBuilder<IdentityPolicy> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.Name)
            .HasMaxLength(MaxLengthValues.PolicyName)
            .IsUnicode(false);

        entity.Property(e => e.Tags)
            .HasMaxLength(MaxLengthValues.Tags)
            .IsUnicode(false);
    }
}