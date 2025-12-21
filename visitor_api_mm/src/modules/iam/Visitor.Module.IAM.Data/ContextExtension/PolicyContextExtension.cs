namespace Visitor.Module.IAM.Data.ContextExtension.IAM;

public static class PolicyContextExtension
{
    public static void CreatePolicy(this IAMServiceContext _dbContext, IdentityPolicy dto)
    {
        _dbContext.AddEntity(dto);
    }

    public static void UpdatePolicy(this IAMServiceContext _dbContext, IdentityPolicy dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static void DeletePolicy(this IAMServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<IdentityPolicy>(Id);
    }

    public static async Task<IdentityPolicyDetail> GetPolicyByIdAsync(this IAMServiceContext _dbContext, Guid id)
    {
        var sql = @"
                    SELECT Id, Name, Tags, UpdatedAt, UpdatedBy, Act_Ind
                    FROM iam.IdentityPolicy 
                    WHERE Id = @Id AND IsDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();

        var raw = await connection.QueryFirstOrDefaultAsync<(Guid Id, string Name, string Tags, DateTime UpdatedAt, string UpdatedBy, int Act_Ind)>(
            sql, new { Id = id });

        if (raw.Equals(default)) return null!;

        return new IdentityPolicyDetail
        {
            Id = raw.Id,
            Name = raw.Name,
            Tags = BaseDbContextExtension.ConvertTags(raw.Tags),
            UpdatedAt = raw.UpdatedAt,
            UpdatedBy = raw.UpdatedBy,
            Act_Ind = raw.Act_Ind
        };
    }

    public static async Task<PaginatedList<IdentityPolicyList>> GetPolicyListAsync(this IAMServiceContext _dbContext, IdentityPolicy dto, int index, int size)
    {
        var whereClause = new StringBuilder("WHERE IsDeleted = FALSE");
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(dto.Name))
        {
            whereClause.Append(" AND LOWER(Name) = @Name");
            parameters.Add("Name", dto.Name.ToLower());
        }

        parameters.Add("Offset", (index - 1) * size);
        parameters.Add("PageSize", size);
        var sql = $@"
                      SELECT Id, Name, Tags, UpdatedAt, UpdatedBy, Act_Ind
                      FROM iam.IdentityPolicy 
                      {whereClause}
                      ORDER BY UpdatedAt DESC
                      OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                      SELECT COUNT(1)
                      FROM iam.IdentityPolicy
                      {whereClause}";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<IdentityPolicy>();
        var totalCount = await multi.ReadFirstAsync<int>();

        var results = rawResults.Select(r => new IdentityPolicyList
        {
            Id = r.Id,
            Name = r.Name,
            Tags = BaseDbContextExtension.ConvertTags(r.Tags),
            UpdatedAt = r.UpdatedAt,
            UpdatedBy = r.UpdatedBy,
            Act_Ind = r.Act_Ind
        }).ToList();

        return results.ToList().ToPaginatedList(index, size, totalCount);
    }


    public static async Task<IdentityPolicy> GetPolicyByNameAsync(this IAMServiceContext _dbContext, string Policy_Nm)
    {
        System.Linq.Expressions.Expression<Func<IdentityPolicy, bool>> predicate = a => a.Name == Policy_Nm;
        var policyDetails = await _dbContext.IdentityPolicy.AsNoTracking().FirstOrDefaultAsync(predicate);
        return policyDetails!;
    }
}
