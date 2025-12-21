namespace Visitor.Module.IAM.Data.ContextExtension.IAM;

public static class PolicyPageActionContextExtension
{
    public static void CreatePolicyPageAction(this IAMServiceContext _dbContext, IdentityPolicyPageActionMapping dto)
    {
        _dbContext.AddEntity(dto);
    }

    public static void UpdatePolicyPageAction(this IAMServiceContext _dbContext, IdentityPolicyPageActionMapping dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static void DeletePolicyPageAction(this IAMServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<IdentityPolicyPageActionMapping>(Id);
    }

    public static async Task<IdentityPolicyPageActionMappingDetail> GetPolicyPageActionByIdAsync(this IAMServiceContext _dbContext, Guid id)
    {
        var sql = @"
        SELECT ppa.Id,
               p.Name AS Policy_Nm, 
               pa.Name AS PageAction_Nm,
               ppa.UpdatedAt, ppa.UpdatedBy, ppa.Act_Ind
        FROM iam.IdentityPolicyPageActionMapping ppa
        JOIN iam.IdentityPolicy p ON ppa.Policy_Id = p.Id
        JOIN iam.IdentityPageAction pa ON ppa.PageAction_Id = pa.Id
        WHERE ppa.Id = @Id AND ppa.IsDeleted = FALSE AND pa.IsDeleted = FALSE AND p.IsDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();
        var result = await connection.QueryFirstOrDefaultAsync<IdentityPolicyPageActionMappingDetail>(sql, new { Id = id });

        return result!;
    }

    public static async Task<PaginatedList<IdentityPolicyPageActionMappingList>> GetPolicyPageActionListAsync(this IAMServiceContext _dbContext, IdentityPolicyPageActionMapping dto, int index, int size)
    {
        var whereClause = new StringBuilder("WHERE ppa.IsDeleted = FALSE AND pa.IsDeleted = FALSE AND p.IsDeleted = FALSE");
        var parameters = new DynamicParameters();

        if (dto.Policy_Id != Guid.Empty)
        {
            whereClause.Append(" AND ppa.Policy_Id = @PolicyId");
            parameters.Add("PolicyId", dto.Policy_Id);
        }

        if (dto.PageAction_Id != Guid.Empty)
        {
            whereClause.Append(" AND ppa.PageAction_Id = @PageActionId");
            parameters.Add("PageActionId", dto.PageAction_Id);
        }

        parameters.Add("Offset", (index - 1) * size);
        parameters.Add("PageSize", size);
        var sql = $@"
                    SELECT ppa.Id,
                           p.Name AS Policy_Nm, 
                           pa.Name AS PageAction_Nm,
                           ppa.UpdatedAt, ppa.UpdatedBy, ppa.Act_Ind
                    FROM iam.IdentityPolicyPageActionMapping ppa
                    JOIN iam.IdentityPolicy p ON ppa.Policy_Id = p.Id
                    JOIN iam.IdentityPageAction pa ON ppa.PageAction_Id = pa.Id
                    {whereClause}
                    ORDER BY ppa.UpdatedAt DESC
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                    SELECT COUNT(1)
                    FROM iam.IdentityPolicyPageActionMapping ppa
                    JOIN iam.IdentityPolicy p ON ppa.Policy_Id = p.Id
                    JOIN iam.IdentityPageAction pa ON ppa.PageAction_Id = pa.Id
                    {whereClause}";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<IdentityPolicyPageActionMappingList>();
        var totalCount = await multi.ReadFirstAsync<int>();

        return rawResults.ToList().ToPaginatedList(index, size, totalCount);
    }

    public static async Task<bool> IsPolicyPageActionMappingExistsAsync(this IAMServiceContext _dbContext, Guid policy_id, Guid pageAction_id)
    {
        System.Linq.Expressions.Expression<Func<IdentityPolicyPageActionMapping, bool>> predicate = a => a.Policy_Id == policy_id & a.PageAction_Id == pageAction_id;
        var exist = await _dbContext.IdentityPolicyPageActionMapping.AsNoTracking().AnyAsync(predicate);
        return exist;
    }
}
