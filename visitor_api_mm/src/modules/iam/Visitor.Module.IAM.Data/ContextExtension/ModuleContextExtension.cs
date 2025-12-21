namespace Visitor.Module.IAM.Data.ContextExtension.IAM;

public static class ModuleContextExtension
{
    public static void CreateModule(this IAMServiceContext _dbContext, IdentityModule dto)
    {
        _dbContext.AddEntity(dto);
    }

    public static void UpdateModule(this IAMServiceContext _dbContext, IdentityModule dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static void DeleteModule(this IAMServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<IdentityModule>(Id);
    }

    // For External Use We will use Dapper Framework
    public static async Task<IdentityModuleDetail> GetModuleByIdAsync(this IAMServiceContext _dbContext, Guid id)
    {
        var sql = @"SELECT 
                           Id, Name, Tags,
                           UpdatedAt, UpdatedBy, Act_Ind
                FROM iam.IdentityModule  
                WHERE Id = @Id AND IsDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();

        var raw = await connection.QueryFirstOrDefaultAsync<(Guid Id, string Name, string Tags, DateTime UpdatedAt, string UpdatedBy, int Act_Ind)>(sql, new { Id = id });

        if (raw.Equals(default)) return null!;

        return new IdentityModuleDetail
        {
            Id = raw.Id,
            Name = raw.Name,
            Tags = BaseDbContextExtension.ConvertTags(raw.Tags),
            UpdatedAt = raw.UpdatedAt,
            UpdatedBy = raw.UpdatedBy,
            Act_Ind = raw.Act_Ind
        };
    }

    public static async Task<PaginatedList<IdentityModuleList>> GetModuleListAsync(this IAMServiceContext _dbContext, IdentityModule dto, int index, int size)
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
        SELECT Id, Name, Tags, Act_Ind, UpdatedAt, UpdatedBy
        FROM iam.IdentityModule
        {whereClause}
        ORDER BY UpdatedAt DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM iam.IdentityModule
        {whereClause};
    ";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<IdentityModule>();
        var totalCount = await multi.ReadFirstAsync<int>();

        var results = rawResults.Select(r => new IdentityModuleList
        {
            Id = r.Id,
            Name = r.Name,
            Tags = BaseDbContextExtension.ConvertTags(r.Tags),
            UpdatedAt = r.UpdatedAt,
            UpdatedBy = r.UpdatedBy,
            Act_Ind = r.Act_Ind
        }).ToList();

        return results.ToPaginatedList(index, size, totalCount);
    }

    // For Internal Use We will use Entity Framework
    public static async Task<IdentityModule> GetModuleByNameAsync(this IAMServiceContext _dbContext, string Module_Nm)
    {
        System.Linq.Expressions.Expression<Func<IdentityModule, bool>> predicate = a => a.Name == Module_Nm;
        var moduleDetails = await _dbContext.IdentityModule.AsNoTracking().FirstOrDefaultAsync(predicate);
        return moduleDetails!;
    }

    ////public static async Task<IdentityModule> GetModuleByIdAsync(this IAMServiceContext _dbContext, Guid id)
    ////{
    ////    System.Linq.Expressions.Expression<Func<IdentityModule, bool>> predicate = a => a.Id == id;
    ////    var moduleDetails = await _dbContext.Modules.AsNoTracking().FirstOrDefaultAsync(predicate);
    ////    return moduleDetails;
    ////}
} 
