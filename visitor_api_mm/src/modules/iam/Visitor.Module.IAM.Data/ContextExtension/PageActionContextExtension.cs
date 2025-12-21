namespace Visitor.Module.IAM.Data.ContextExtension.IAM;

public static class PageActionContextExtension
{
    public static void CreatePageAction(this IAMServiceContext _dbContext, IdentityPageAction dto)
    {
        _dbContext.AddEntity(dto);
    }

    public static void UpdatePageAction(this IAMServiceContext _dbContext, IdentityPageAction dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static void DeletePageAction(this IAMServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<IdentityPageAction>(Id);
    }

    public static async Task<IdentityPageActionDetail> GetPageActionByIdAsync(this IAMServiceContext _dbContext, Guid id)
    {
        var sql = @"
        SELECT pa.Id, pa.Name, pa.Action, pa.AccessLevel, pa.PageUrl,
               p.Page_Nm,
               pa.UpdatedAt, pa.UpdatedBy, pa.Act_Ind
        FROM iam.IdentityPageAction pa 
        JOIN iam.IdentityPage p ON pa.Page_Id = p.Id
        WHERE pa.Id = @Id AND pa.IsDeleted = FALSE AND p.IsDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();

        var result = await connection.QueryFirstOrDefaultAsync<IdentityPageActionDetail>(sql, new { Id = id });

        return result!;
    }

    public static async Task<PaginatedList<IdentityPageActionList>> GetPageActionListAsync(this IAMServiceContext _dbContext, IdentityPageAction dto, int index, int size)
    {
        var whereClause = new StringBuilder("WHERE pa.IsDeleted = FALSE AND p.IsDeleted = FALSE");
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(dto.Name))
        {
            whereClause.Append(" AND LOWER(p.Name) = @Name");
            parameters.Add("Name", dto.Name.ToLower());
        }

        if (!string.IsNullOrEmpty(dto.Action))
        {
            whereClause.Append(" AND LOWER(p.Action) = @Action");
            parameters.Add("Action", dto.Action.ToLower());
        }

        parameters.Add("Offset", (index - 1) * size);
        parameters.Add("PageSize", size);

        var sql = $@"
                    SELECT pa.Id, pa.Name, pa.Action, pa.AccessLevel, pa.PageUrl,
                           p.Page_Nm,
                           pa.UpdatedAt, pa.UpdatedBy, pa.Act_Ind
                    FROM iam.IdentityPageAction pa 
                    JOIN iam.IdentityPage p ON pa.Page_Id = p.Id
                     {whereClause}
                     ORDER BY p.UpdatedAt DESC
                     OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                     SELECT COUNT(1)
                     FROM iam.IdentityPageAction pa 
                     JOIN iam.IdentityPage p ON pa.Page_Id = p.Id
                     {whereClause};
                     ";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<IdentityPageActionList>();
        var totalCount = await multi.ReadFirstAsync<int>();

        return rawResults.ToList().ToPaginatedList(index, size, totalCount);
    }

    public static async Task<IdentityPageAction> GetPageActionByNameAsync(this IAMServiceContext _dbContext, string Name)
    {
        System.Linq.Expressions.Expression<Func<IdentityPageAction, bool>> predicate = a => a.Name == Name;
        var moduleDetails = await _dbContext.IdentityPageAction.AsNoTracking().FirstOrDefaultAsync(predicate);
        return moduleDetails!;
    }

    public static async Task<List<IdentityPageActionList>> GetPageActionListAsync(this IAMServiceContext _dbContext, IdentityPageAction dto)
    {
        var baseQuery = _dbContext.IdentityPageAction.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(dto.Name))
            baseQuery = baseQuery.Where(x => x.Name == dto.Name);

        if (!string.IsNullOrEmpty(dto.Action))
            baseQuery = baseQuery.Where(x => x.Action == dto.Action);

        if (dto.Page_Id != Guid.Empty)
            baseQuery = baseQuery.Where(x => x.Page_Id == dto.Page_Id);

        var joinedQuery = from a in baseQuery
                          join p in _dbContext.IdentityPage.AsNoTracking()
                          on a.Page_Id equals p.Id
                          select new { a, p };

        return await joinedQuery.OrderByDescending(x => x.a.UpdatedAt)
                        .Select(x => new IdentityPageActionList
                        {
                            Id = x.a.Id,
                            Name = x.a.Name,
                            Action = x.a.Action,
                            AccessLevel = x.a.AccessLevel,
                            PageUrl = x.a.PageUrl,
                            Page_Nm = x.p.Page_Nm,
                            Act_Ind = x.a.Act_Ind,
                            UpdatedAt = x.a.UpdatedAt,
                            UpdatedBy = x.a.UpdatedBy
                        })
                        .ToListAsync();
    }
}
