using System.Text.Json;

namespace Visitor.Module.DMS.Data.ContextExtension;

public static class DocumentContextExtension
{
    public static void CreateDocument(this DMSServiceContext _dbContext, Document dto)
    {
        _dbContext.AddEntity(dto);
    }

    public static void UpdateDocument(this DMSServiceContext _dbContext, Document dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static void DeleteDocument(this DMSServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<Document>(Id);
    }

    public static async Task<DocumentDetail> GetDocumentByIdAsync(this DMSServiceContext _dbContext, Guid id)
    {
        const string sql = @" SELECT 
            d.Id, d.Reference_No, d.Document_Nm, 
            dc.Name AS Document_Category, dt.Name AS Document_Type,
            d.Document_Cd, d.Document_No, d.Document_Desc, d.Document_Version,
            d.Source_System, d.Filter_Year,
            d.Content,  d.Metadata,
            d.InternalStream,  d.InternalPath, d.InternalExtension,
            d.Param1, d.Param2, d.Param3, d.Param4, d.Param5,
            d.Date1, d.Date2, d.Date3, d.Date4, d.Date5,
            d.Tags,
            d.UpdatedAt, d.UpdatedBy, d.Act_Ind
        FROM dms.Document d
        JOIN dms.DocumentCategory dc ON d.Document_Category_Id = dc.Id
        JOIN dms.DocumentType dt ON d.Document_Type_Id = dt.Id
        WHERE d.Id = @Id AND d.IsDeleted = FALSE AND dc.IsDeleted = FALSE AND dt.IsDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();

        var raw = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });

        if (raw == null)
            return null;

        return new DocumentDetail
        {
            Id = raw.Id,
            Reference_No = raw.Reference_No,
            Document_Nm = raw.Document_Nm,
            Document_Category = raw.Document_Category,
            Document_Type = raw.Document_Type,
            Document_Cd = raw.Document_Cd,
            Document_No = raw.Document_No,
            Document_Desc = raw.Document_Desc,
            Document_Version = raw.Document_Version,
            Source_System = raw.Source_System,
            Filter_Year = raw.Filter_Year,
            Content = JsonSerializer.Deserialize<FileAttributeDetails>(raw.Content ?? "{}"),
            Metadata = BaseDbContextExtension.ConvertTags(raw.Metadata ?? "{}"),
            InternalStream = raw.InternalStream,
            InternalPath = raw.InternalPath,
            InternalExtension = raw.InternalExtension,
            Param1 = raw.Param1,
            Param2 = raw.Param2,
            Param3 = raw.Param3,
            Param4 = raw.Param4,
            Param5 = raw.Param5,
            Date1 = raw.Date1,
            Date2 = raw.Date2,
            Date3 = raw.Date3,
            Date4 = raw.Date4,
            Date5 = raw.Date5,
            Tags = BaseDbContextExtension.ConvertTags(raw.Tags ?? "{}"),
            UpdatedAt = raw.UpdatedAt,
            UpdatedBy = raw.UpdatedBy,
            Act_Ind = raw.Act_Ind
        };
    }

    public static async Task<PaginatedList<DocumentList>> GetModuleListAsync(this DMSServiceContext _dbContext, Document dto, int index, int size)
    {
        var whereClause = new StringBuilder("WHERE d.IsDeleted = FALSE AND dc.IsDeleted = FALSE AND dt.IsDeleted = FALSE");
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(dto.Reference_No))
        {
            whereClause.Append(" AND LOWER(d.Reference_No) = @Reference_No");
            parameters.Add("Reference_No", dto.Reference_No.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(dto.Document_Nm))
        {
            whereClause.Append(" AND LOWER(d.Document_Nm) = @Document_Nm");
            parameters.Add("Document_Nm", dto.Document_Nm.ToLower());
        }

        if (dto.Document_Category_Id != Guid.Empty)
        {
            whereClause.Append(" AND d.Document_Category_Id = @Document_Category_Id");
            parameters.Add("Document_Category_Id", dto.Document_Category_Id);
        }

        if (dto.Document_Type_Id != Guid.Empty)
        {
            whereClause.Append(" AND d.Document_Type_Id = @Document_Type_Id");
            parameters.Add("Document_Type_Id", dto.Document_Type_Id);
        }

        if (!string.IsNullOrWhiteSpace(dto.Document_Cd))
        {
            whereClause.Append(" AND LOWER(d.Document_Cd) = @Document_Cd");
            parameters.Add("Document_Cd", dto.Document_Cd.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(dto.Document_No))
        {
            whereClause.Append(" AND LOWER(d.Document_No) = @Document_No");
            parameters.Add("Document_No", dto.Document_No.ToLower());
        }

        if (dto.Document_Version > 0)
        {
            whereClause.Append(" AND d.Document_Version = @Document_Version");
            parameters.Add("Document_Version", dto.Document_Version);
        }

        if (!string.IsNullOrWhiteSpace(dto.Source_System))
        {
            whereClause.Append(" AND LOWER(d.Source_System) = @Source_System");
            parameters.Add("Source_System", dto.Source_System.ToLower());
        }

        if (dto.Filter_Year > 0)
        {
            whereClause.Append(" AND d.Filter_Year = @Filter_Year");
            parameters.Add("Filter_Year", dto.Filter_Year);
        }

        parameters.Add("Offset", (index - 1) * size);
        parameters.Add("PageSize", size);

        var sql = $@"
        SELECT 
            d.Id, d.Reference_No, d.Document_Nm, 
            dc.Name AS Document_Category, dt.Name AS Document_Type,
            d.Document_Cd, d.Document_No, d.Document_Desc, d.Document_Version,
            d.Source_System, d.Filter_Year,
            d.Content,  d.Metadata,
            d.InternalStream,  d.InternalPath, d.InternalExtension,
            d.Param1, d.Param2, d.Param3, d.Param4, d.Param5,
            d.Date1, d.Date2, d.Date3, d.Date4, d.Date5,
            d.Tags,
            d.UpdatedAt, d.UpdatedBy, d.Act_Ind
        FROM dms.Document d
        JOIN dms.DocumentCategory dc ON d.Document_Category_Id = dc.Id
        JOIN dms.DocumentType dt ON d.Document_Type_Id = dt.Id
        {whereClause}
        ORDER BY UpdatedAt DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM dms.Document d
        JOIN dms.DocumentCategory dc ON d.Document_Category_Id = dc.Id
        JOIN dms.DocumentType dt ON d.Document_Type_Id = dt.Id
        {whereClause};";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<DocumentList>();
        var totalCount = await multi.ReadFirstAsync<int>();

        return rawResults.ToList().ToPaginatedList(index, size, totalCount);
    }
}
