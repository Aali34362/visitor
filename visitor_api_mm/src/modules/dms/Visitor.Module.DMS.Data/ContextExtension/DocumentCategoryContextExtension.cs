namespace Visitor.Module.DMS.Data.ContextExtension;

public static class DocumentCategoryContextExtension
{
    public static void CreateDocumentCategory(this DMSServiceContext _dbContext, DocumentCategory dto)
    {
        _dbContext.AddEntity(dto);
    }

    public static void UpdateDocumentCategory(this DMSServiceContext _dbContext, DocumentCategory dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static void DeleteDocumentCategory(this DMSServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<DocumentCategory>(Id);
    }

    public static async Task<DocumentCategoryDetail> GetDocumentCategoryByIdAsync(this DMSServiceContext _dbContext, Guid id)
    {
        var sql = @"SELECT 
                           Id, Name, Tags,
                           UpdatedAt, UpdatedBy, Act_Ind
                FROM dms.DocumentCategory  
                WHERE Id = @Id AND IsDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();

        var raw = await connection.QueryFirstOrDefaultAsync<(Guid Id, string Name, string Tags, DateTime UpdatedAt, string UpdatedBy, int Act_Ind)>(sql, new { Id = id });

        if (raw.Equals(default)) return null!;

        return new DocumentCategoryDetail
        {
            Id = raw.Id,
            Name = raw.Name,
            Tags = BaseDbContextExtension.ConvertTags(raw.Tags),
            UpdatedAt = raw.UpdatedAt,
            UpdatedBy = raw.UpdatedBy,
            Act_Ind = raw.Act_Ind
        };
    }

    public static async Task<PaginatedList<DocumentCategoryList>> GetDocumentCategoryListAsync(this DMSServiceContext _dbContext, DocumentCategory dto, int index, int size)
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
        FROM dms.DocumentCategory
        {whereClause}
        ORDER BY UpdatedAt DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM dms.DocumentCategory
        {whereClause};
    ";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<DocumentCategory>();
        var totalCount = await multi.ReadFirstAsync<int>();

        var results = rawResults.Select(r => new DocumentCategoryList
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

    public static async Task<DocumentCategory> GetDocumentCategoryByNameAsync(this DMSServiceContext _dbContext, string Category_Nm)
    {
        System.Linq.Expressions.Expression<Func<DocumentCategory, bool>> predicate = a => a.Name == Category_Nm;
        var categoryDetails = await _dbContext.DocumentCategory.AsNoTracking().FirstOrDefaultAsync(predicate);
        return categoryDetails!;
    }
}
