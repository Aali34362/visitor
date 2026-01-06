namespace Visitor.Module.IAM.Data.ContextExtension.IAM;

public static class RoleContextExtension
{
    public static void CreateRole(this IAMServiceContext _dbContext, IdentityRole dto)
    {
        _dbContext.AddEntity(dto);
    }

    public static void UpdateRole(this IAMServiceContext _dbContext, IdentityRole dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static void DeleteRole(this IAMServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<IdentityRole>(Id);
    }

    public static async Task<IdentityRoleDetail> GetRoleByIdAsync(this IAMServiceContext _dbContext, Guid id)
    {
        var sql = @"
                    SELECT Id, Name, Tags, UpdatedAt, UpdatedBy, Act_Ind
                    FROM iam.IdentityRole
                    WHERE Id = @Id AND IsDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();

        var raw = await connection.QueryFirstOrDefaultAsync<(Guid Id, string Name, string Tags, DateTime UpdatedAt, string UpdatedBy, int Act_Ind)>(
            sql, new { Id = id });

        if (raw.Equals(default)) return null!;

        return new IdentityRoleDetail
        {
            id = raw.Id,
            name = raw.Name,
            tags = BaseDbContextExtension.ConvertTags(raw.Tags),
            updated_At = raw.UpdatedAt,
            updated_By = raw.UpdatedBy,
            act_Ind = raw.Act_Ind
        };
    }

    public static async Task<PaginatedList<IdentityRoleList>> GetRoleListAsync(this IAMServiceContext _dbContext, IdentityRole dto, int index, int size)
    {
        var whereClause = new StringBuilder("WHERE IsDeleted = FALSE");
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(dto.name))
        {
            whereClause.Append(" AND LOWER(Name) = @Name");
            parameters.Add("Name", dto.name.ToLower());
        }

        parameters.Add("Offset", (index - 1) * size);
        parameters.Add("PageSize", size);
        var sql = $@"
                      SELECT Id, Name, Tags, UpdatedAt, UpdatedBy, Act_Ind
                      FROM iam.IdentityRole
                      {whereClause}
                      ORDER BY UpdatedAt DESC
                      OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                      SELECT COUNT(1)
                      FROM iam.IdentityRole
                      {whereClause}";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<IdentityRole>();
        var totalCount = await multi.ReadFirstAsync<int>();

        var results = rawResults.Select(r => new IdentityRoleList
        {
            id = r.id,
            name = r.name,
            tags = BaseDbContextExtension.ConvertTags(r.tags),
            updated_At = r.updated_At,
            updated_By = r.updated_By,
            act_Ind = r.act_Ind
        }).ToList();

        return results.ToList().ToPaginatedList(index, size, totalCount);
    }

    public static async Task<IdentityRole> GetRoleByNameAsync(this IAMServiceContext _dbContext, string Role_Nm)
    {
        System.Linq.Expressions.Expression<Func<IdentityRole, bool>> predicate = a => a.name == Role_Nm;
        var roleDetails = await _dbContext.IdentityRole.AsNoTracking().FirstOrDefaultAsync(predicate);
        return roleDetails!;
    }
}
