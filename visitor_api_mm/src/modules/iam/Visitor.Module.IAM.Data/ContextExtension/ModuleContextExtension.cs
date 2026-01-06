namespace Visitor.Module.IAM.Data.ContextExtension.IAM;

public static class ModuleEfExtensions
{
    public static void Create(this IAMServiceContext db, IdentityModule entity)
        => db.AddEntity(entity);

    public static void Update(this IAMServiceContext db, IdentityModule entity)
        => db.UpdateEntity(entity);

    public static void Delete(this IAMServiceContext db, Guid id)
        => db.DeleteEntity<IdentityModule>(id);
}

public static class ModuleDapperQueries
{
    private const string NotDeletedClause = "is_deleted = false";

    #region Single Module
    public static async Task<IdentityModuleDetail?> GetModuleByIdAsync(this IAMServiceContext dbContext, Guid id)
    {
        const string sql = @"
                SELECT  id, name, tags,
                    updated_at, updated_by, act_ind
                FROM iam.identity_module
                WHERE id = @Id
                AND is_deleted = false;
            ";
        /*
         * SQL query definition
           What’s happening
            This is a raw SQL query written as a verbatim string (@"...")
            It selects specific columns from the table iam.IdentityModule
           Filters rows using:
            id = @Id
            is_Deleted = FALSE
           Important details:
            @Id is a parameter placeholder
            Dapper will safely replace @Id with a value (prevents SQL injection)
            Only non-deleted records are fetched (soft delete pattern)
         */

        var connection = dbContext.Database.GetDbConnection();
        /*
         * Getting the database connection
           What’s happening
            _dbContext is an Entity Framework Core DbContext
            GetDbConnection() extracts the underlying ADO.NET connection
            e.g. SqlConnection, NpgsqlConnection, etc.
           Why this is useful:
            You’re reusing EF Core’s connection
            No need to manage connection strings twice
            Allows EF Core + Dapper together in the same project (very common pattern)
         */

        var raw = await connection.QueryFirstOrDefaultAsync<IdentityModule>(sql, new { Id = id });

        if (raw is null) return null;

        return MapToDetail(raw);
    }
    #endregion

    #region List / Pagination
    public static async Task<PaginatedList<IdentityModuleList>> GetModuleListAsync(
        this IAMServiceContext dbContext,
        IdentityModule dto,
        int pageIndex,
        int pageSize)
    {
        var whereClause = new StringBuilder($"WHERE {NotDeletedClause}");
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(dto.name))
        {
            whereClause.Append(" AND LOWER(name) = @Name");
            parameters.Add("Name", dto.name.ToLower());
        }

        parameters.Add("Offset", (pageIndex - 1) * pageSize);
        parameters.Add("PageSize", pageSize);

        var sql = $@"
                SELECT id, name, tags,
                act_ind, updated_at, updated_by
                FROM iam.identity_module
                {whereClause}
                ORDER BY updated_at DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(1)
                FROM iam.identity_module
                {whereClause};
            ";

        var connection = dbContext.Database.GetDbConnection();

        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rows = (await multi.ReadAsync<IdentityModule>()).ToList();
        var totalCount = await multi.ReadFirstAsync<int>();

        var items = rows.Select(MapToList).ToList();

        return items.ToPaginatedList(pageIndex, pageSize, totalCount);
    }
    #endregion

    #region Mapping
    private static IdentityModuleDetail MapToDetail(IdentityModule row)
    {
        return new IdentityModuleDetail
        {
            id = row.id,
            name = row.name,
            tags = BaseDbContextExtension.ConvertTags(row.tags),
            updated_At = row.updated_At,
            updated_By = row.updated_By,
            act_Ind = row.act_Ind
        };
    }
    private static IdentityModuleList MapToList(IdentityModule row)
    {
        return new IdentityModuleList
        {
            id = row.id,
            name = row.name,
            tags = BaseDbContextExtension.ConvertTags(row.tags),
            updated_At = row.updated_At,
            updated_By = row.updated_By,
            act_Ind = row.act_Ind
        };
    }
    #endregion
}

public static class ModuleEfQueries
{
    public static async Task<IdentityModule?> GetByIdForUpdateAsync(
        this IAMServiceContext dbContext,
        Guid id)
    {
        return await dbContext.IdentityModule
            .FirstOrDefaultAsync(x =>
                x.id == id &&
                !x.is_Deleted);
    }

    public static async Task<IdentityModule?> GetByNameForValidationAsync(
        this IAMServiceContext dbContext,
        string name)
    {
        return await dbContext.IdentityModule
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.name == name &&
                !x.is_Deleted);
    }
}

////public static class ModuleContextExtension
////{
////    public static void CreateModule(this IAMServiceContext _dbContext, IdentityModule dto)
////    {
////        _dbContext.AddEntity(dto);
////    }

////    public static void UpdateModule(this IAMServiceContext _dbContext, IdentityModule dto)
////    {
////        _dbContext.UpdateEntity(dto);
////    }

////    public static void DeleteModule(this IAMServiceContext _dbContext, Guid Id)
////    {
////        _dbContext.DeleteEntity<IdentityModule>(Id);
////    }

////    // For External Use We will use Dapper Framework
////    public static async Task<IdentityModuleDetail> GetModuleByIdAsync(this IAMServiceContext _dbContext, Guid id)
////    {
////        var sql = @"SELECT 
////                id, name, tags,
////                updated_At, updated_By, act_Ind
////                FROM iam.IdentityModule  
////                WHERE id = @Id AND is_Deleted = FALSE";
////        /*
////         * SQL query definition
////           What’s happening
////            This is a raw SQL query written as a verbatim string (@"...")
////            It selects specific columns from the table iam.IdentityModule
////           Filters rows using:
////            id = @Id
////            is_Deleted = FALSE
////           Important details:
////            @Id is a parameter placeholder
////            Dapper will safely replace @Id with a value (prevents SQL injection)
////            Only non-deleted records are fetched (soft delete pattern)
////         */

////        var connection = _dbContext.Database.GetDbConnection();
////        /*
////         * Getting the database connection
////           What’s happening
////            _dbContext is an Entity Framework Core DbContext
////            GetDbConnection() extracts the underlying ADO.NET connection
////            e.g. SqlConnection, NpgsqlConnection, etc.
////           Why this is useful:
////            You’re reusing EF Core’s connection
////            No need to manage connection strings twice
////            Allows EF Core + Dapper together in the same project (very common pattern)
////         */

////        var raw = await connection.QueryFirstOrDefaultAsync<(Guid Id, string Name, string Tags, DateTime updated_At, string updated_By, int act_Ind)>(sql, new { Id = id });

////        if (raw.Equals(default)) return null!;

////        return new IdentityModuleDetail
////        {
////            id = raw.Id,
////            name = raw.Name,
////            tags = BaseDbContextExtension.ConvertTags(raw.Tags),
////            updated_At = raw.updated_At,
////            updated_By = raw.updated_By,
////            act_Ind = raw.act_Ind
////        };
////    }

////    public static async Task<PaginatedList<IdentityModuleList>> GetModuleListAsync(this IAMServiceContext _dbContext, IdentityModule dto, int index, int size)
////    {
////        var whereClause = new StringBuilder("WHERE is_Deleted = FALSE");
////        var parameters = new DynamicParameters();

////        if (!string.IsNullOrEmpty(dto.name))
////        {
////            whereClause.Append(" AND LOWER(Name) = @Name");
////            parameters.Add("Name", dto.name.ToLower());
////        }

////        parameters.Add("Offset", (index - 1) * size);
////        parameters.Add("PageSize", size);

////        var sql = $@"
////        SELECT id, name, tags, 
////        act_Ind, updated_At, updated_By
////        FROM iam.IdentityModule
////        {whereClause}
////        ORDER BY updated_At DESC
////        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

////        SELECT COUNT(1)
////        FROM iam.IdentityModule
////        {whereClause};
////        ";

////        var connection = _dbContext.Database.GetDbConnection();
////        using var multi = await connection.QueryMultipleAsync(sql, parameters);

////        var rawResults = await multi.ReadAsync<IdentityModule>();
////        var totalCount = await multi.ReadFirstAsync<int>();

////        var results = rawResults.Select(r => new IdentityModuleList
////        {
////            id = r.id,
////            name = r.name,
////            tags = BaseDbContextExtension.ConvertTags(r.tags),
////            updated_At = r.updated_At,
////            updated_By = r.updated_By,
////            act_Ind = r.act_Ind
////        }).ToList();

////        return results.ToPaginatedList(index, size, totalCount);
////    }

////    // For Internal Use We will use Entity Framework
////    public static async Task<IdentityModule> GetModuleByNameAsync(this IAMServiceContext _dbContext, string Module_Nm)
////    {
////        System.Linq.Expressions.Expression<Func<IdentityModule, bool>> predicate = a => a.name == Module_Nm;
////        var moduleDetails = await _dbContext.IdentityModule.AsNoTracking().FirstOrDefaultAsync(predicate);
////        return moduleDetails!;
////    }

////    ////public static async Task<IdentityModule> GetModuleByIdAsync(this IAMServiceContext _dbContext, Guid id)
////    ////{
////    ////    System.Linq.Expressions.Expression<Func<IdentityModule, bool>> predicate = a => a.Id == id;
////    ////    var moduleDetails = await _dbContext.Modules.AsNoTracking().FirstOrDefaultAsync(predicate);
////    ////    return moduleDetails;
////    ////}
////} 
