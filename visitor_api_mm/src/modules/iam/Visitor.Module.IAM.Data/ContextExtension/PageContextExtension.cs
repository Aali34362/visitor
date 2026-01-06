namespace Visitor.Module.IAM.Data.ContextExtension.IAM;

public static class PageContextExtension
{
    public static void CreatePage(this IAMServiceContext _dbContext, IdentityPage dto)
    {
        _dbContext.AddEntity(dto);
    }

    public static void UpdatePage(this IAMServiceContext _dbContext, IdentityPage dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static void DeletePage(this IAMServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<IdentityPage>(Id);
    }

    public static async Task<IdentityPageDetail> GetPageByIdAsync(this IAMServiceContext _dbContext, Guid id)
    {
        var sql = @"
        SELECT p.Id, p.Parent_Id,  p.Page_Level, p.Page_Title, p.Page_Url, p.Page_Order, p.Page_Nm, p.Icon,
               p.Module_Id, m.Name AS Module_Nm, 
               p.UpdatedAt, p.UpdatedBy, p.Act_Ind
        FROM iam.IdentityPage p
        JOIN iam.IdentityModule m ON p.Module_Id = m.Id
        WHERE p.Id = @Id AND p.IsDeleted = FALSE AND m.IsDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();

        var result = await connection.QueryFirstOrDefaultAsync<IdentityPageDetail>(sql, new { Id = id });

        return result!;
    }

    public static async Task<PaginatedList<IdentityPageList>> GetPageListAsync(this IAMServiceContext _dbContext, IdentityPage dto, int index, int size)
    {
        var whereClause = new StringBuilder("WHERE p.is_Deleted = FALSE AND m.is_Deleted = FALSE");
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(dto.page_Nm))
        {
            whereClause.Append(" AND LOWER(p.Page_Nm) = @Page_Nm");
            parameters.Add("Page_Nm", dto.page_Nm.ToLower());
        }

        if (dto.page_Level > 0)
        {
            whereClause.Append(" AND p.Page_Level = @Page_Level");
            parameters.Add("Page_Level", dto.page_Level);
        }

        if (dto.module_Id != Guid.Empty)
        {
            whereClause.Append(" AND p.Module_Id = @Module_Id");
            parameters.Add("Module_Id", dto.module_Id);
        }

        parameters.Add("Offset", (index - 1) * size);
        parameters.Add("PageSize", size);

        var sql = $@"
                     SELECT  p.Id, p.Parent_Id,  p.Page_Level, p.Page_Title, p.Page_Url, p.Page_Order, p.Page_Nm, p.Icon,
                             p.Module_Id, m.Name AS Module_Nm, 
                             p.UpdatedAt, p.UpdatedBy, p.Act_Ind
                     FROM iam.IdentityPage p
                     JOIN iam.IdentityModule m ON p.Module_Id = m.Id
                     {whereClause}
                     ORDER BY p.UpdatedAt DESC
                     OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                     SELECT COUNT(1)
                     FROM iam.IdentityPage p
                     JOIN iam.IdentityModule m ON p.Module_Id = m.Id
                     {whereClause};
                     ";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<IdentityPageList>();
        var totalCount = await multi.ReadFirstAsync<int>();

        return rawResults.ToList().ToPaginatedList(index, size, totalCount);
    }


    // For Internal Use We will use Entity Framework
    public static async Task<IdentityPage> GetPageByNameAsync(this IAMServiceContext _dbContext, string Page_Nm)
    {
        System.Linq.Expressions.Expression<Func<IdentityPage, bool>> predicate = a => a.page_Nm == Page_Nm;
        var pageDetails = await _dbContext.IdentityPage.AsNoTracking().FirstOrDefaultAsync(predicate);
        return pageDetails!;
    }

    public static async Task<bool> ParentIdExistAsync(this IAMServiceContext _dbContext, Guid Parent_Id)
    {
        System.Linq.Expressions.Expression<Func<IdentityPage, bool>> predicate = a => a.id == Parent_Id;
        var pageDetails = await _dbContext.IdentityPage.AsNoTracking().FirstOrDefaultAsync(predicate);
        if(pageDetails is not null)
            return true;
        return false;
    }
}
