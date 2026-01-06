namespace Visitor.Module.IAM.Migrations.Configuration;

public class IdentityPolicyConfiguration : BasePostGreSqlConfiguration<IdentityPolicy>
{
    public override void Configure(EntityTypeBuilder<IdentityPolicy> entity)
    {
        base.Configure(entity);

        entity.Property(e => e.name)
            .HasMaxLength(MaxLengthValues.PolicyName)
            .IsUnicode(false);

        entity.Property(e => e.tags)
            .HasMaxLength(MaxLengthValues.Tags)
            .IsUnicode(false);
    }
}