namespace Visitor.Module.DMS.Data.ContextExtension;

public static class DocumentTypeContextExtension
{
    public static void CreateDocumentType(this DMSServiceContext _dbContext, DocumentType dto)
    {
        _dbContext.AddEntity(dto);
    }

    public static void UpdateDocumentType(this DMSServiceContext _dbContext, DocumentType dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static void DeleteDocumentType(this DMSServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<DocumentType>(Id);
    }

    public static async Task<DocumentTypeDetail> GetModuleByIdAsync(this DMSServiceContext _dbContext, Guid id)
    {
        var sql = @"SELECT 
                           dt.Id, dt.Name, dt.Tags,
                           dc.Name AS Category_Nm, 
                           dt.UpdatedAt, dt.UpdatedBy, dt.Act_Ind
                FROM dms.DocumentType dt
                JOIN dms.DocumentCategory dc ON dt.Category_Id = dc.Id
                WHERE dt.Id = @Id AND dt.IsDeleted = FALSE AND dc.IsDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();

        var raw = await connection.QueryFirstOrDefaultAsync<(Guid Id, string Name, string Category_Nm, string Tags, DateTime UpdatedAt, string UpdatedBy, int Act_Ind)>(sql, new { Id = id });

        if (raw.Equals(default)) return null!;

        return new DocumentTypeDetail
        {
            Id = raw.Id,
            Name = raw.Name,
            Category_Nm = raw.Category_Nm,
            Tags = BaseDbContextExtension.ConvertTags(raw.Tags),
            UpdatedAt = raw.UpdatedAt,
            UpdatedBy = raw.UpdatedBy,
            Act_Ind = raw.Act_Ind
        };
    }

    public static async Task<PaginatedList<DocumentTypeList>> GetModuleListAsync(this DMSServiceContext _dbContext, DocumentType dto, int index, int size)
    {
        var whereClause = new StringBuilder("WHERE dt.IsDeleted = FALSE AND dc.IsDeleted = FALSE");
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(dto.Name))
        {
            whereClause.Append(" AND LOWER(dt.Name) = @Name");
            parameters.Add("Name", dto.Name.ToLower());
        }

        if (dto.Category_Id != Guid.Empty)
        {
            whereClause.Append(" AND dt.Category_Id = @Category_Id");
            parameters.Add("Category_Id", dto.Category_Id);
        }

        parameters.Add("Offset", (index - 1) * size);
        parameters.Add("PageSize", size);

        var sql = $@"
        SELECT 
              dt.Id, dt.Name, dt.Tags,
              dc.Name AS Category_Nm, 
              dt.UpdatedAt, dt.UpdatedBy, dt.Act_Ind
        FROM dms.DocumentType dt
        JOIN dms.DocumentCategory dc ON dt.Category_Id = dc.Id
        {whereClause}
        ORDER BY UpdatedAt DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM dms.DocumentType dt
        JOIN dms.DocumentCategory dc ON dt.Category_Id = dc.Id
        {whereClause};";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<DocumentType>();
        var totalCount = await multi.ReadFirstAsync<int>();

        var results = rawResults.Select(r => new DocumentTypeList
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
}
