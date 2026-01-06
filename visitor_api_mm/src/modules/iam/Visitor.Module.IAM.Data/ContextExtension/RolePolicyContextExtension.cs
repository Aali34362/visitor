namespace Visitor.Module.IAM.Data.ContextExtension.IAM;

public static class RolePolicyContextExtension
{
    public static void CreateRolePolicy(this IAMServiceContext _dbContext, IdentityRolePolicyMapping dto)
    {
        _dbContext.AddEntity(dto);
    }

    public static void UpdateRolePolicy(this IAMServiceContext _dbContext, IdentityRolePolicyMapping dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static void DeleteRolePolicy(this IAMServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<IdentityRolePolicyMapping>(Id);
    }

    public static async Task<IdentityRolePolicyMappingDetail> GetRolePolicyByIdAsync(this IAMServiceContext _dbContext, Guid id)
    {
        var sql = @"
        SELECT rp.Id,
               p.Name AS Policy_Nm, 
               r.Name AS Role_Nm,
               rp.UpdatedAt, rp.UpdatedBy, rp.Act_Ind
        FROM iam.IdentityRolePolicyMapping rp
        JOIN iam.IdentityPolicy p ON rp.Policy_Id = p.Id
        JOIN iam.IdentityRole r ON rp.Role_Id = r.Id
        WHERE rp.Id = @Id
        AND rp.IsDeleted = FALSE
        AND p.IsDeleted = FALSE
        AND r.IsDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();
        var result = await connection.QueryFirstOrDefaultAsync<IdentityRolePolicyMappingDetail>(sql, new { Id = id });

        return result!;
    }

    public static async Task<PaginatedList<IdentityRolePolicyMappingList>> GetRolePolicyListAsync(this IAMServiceContext _dbContext, IdentityRolePolicyMapping dto, int index, int size)
    {
        var whereClause = new StringBuilder("WHERE rp.IsDeleted = FALSE AND p.IsDeleted = FALSE AND r.IsDeleted = FALSE");
        var parameters = new DynamicParameters();

        if (dto.policy_Id != Guid.Empty)
        {
            whereClause.Append(" AND rp.Policy_Id = @PolicyId");
            parameters.Add("PolicyId", dto.policy_Id);
        }

        if (dto.role_Id != Guid.Empty)
        {
            whereClause.Append(" AND rp.Role_Id = @RoleId");
            parameters.Add("RoleId", dto.role_Id);
        }
        parameters.Add("Offset", (index - 1) * size);
        parameters.Add("PageSize", size);

        var sql = $@"
                      SELECT rp.Id,
                             p.Name AS Policy_Nm, 
                             r.Name AS Role_Nm,
                             rp.UpdatedAt, rp.UpdatedBy, rp.Act_Ind
                      FROM iam.IdentityRolePolicyMapping rp
                      JOIN iam.IdentityPolicy p ON rp.Policy_Id = p.Id
                      JOIN iam.IdentityRole r ON rp.Role_Id = r.Id
                      {whereClause}
                      ORDER BY rp.UpdatedAt DESC
                      OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                        
                      SELECT COUNT(1)
                      FROM iam.IdentityRolePolicyMapping rp
                      JOIN iam.IdentityPolicy p ON rp.Policy_Id = p.Id
                      JOIN iam.IdentityRole r ON rp.Role_Id = r.Id
                      {whereClause}";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<IdentityRolePolicyMappingList>();
        var totalCount = await multi.ReadFirstAsync<int>();

        return rawResults.ToList().ToPaginatedList(index, size, totalCount);
    }

    public static async Task<bool> IsRolePolicyMappingExistsAsync(this IAMServiceContext _dbContext, Guid policy_id, Guid role_id)
    {
        System.Linq.Expressions.Expression<Func<IdentityRolePolicyMapping, bool>> predicate = a => a.policy_Id == policy_id & a.role_Id == role_id;
        var exist = await _dbContext.IdentityRolePolicyMapping.AsNoTracking().AnyAsync(predicate);
        return exist;
    }
}
