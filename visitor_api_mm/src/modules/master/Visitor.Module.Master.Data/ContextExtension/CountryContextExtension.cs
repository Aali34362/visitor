namespace Visitor.Module.Master.Data.ContextExtension;

public static class ModuleContextExtension
{
    public static void CreateCountry(this MasterServiceContext _dbContext, Country dto)
    {
        _dbContext.AddEntity(dto);
    }

    public static void UpdateCountry(this MasterServiceContext _dbContext, Country dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static void DeleteCountry(this MasterServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<Country>(Id);
    }

    // For External Use We will use Dapper Framework
    public static async Task<CountryDetail> GetCountryByIdAsync(this MasterServiceContext _dbContext, Guid id)
    {
        var sql = @"SELECT 
                id, name, code,
                updatedAt, updatedBy, act_Ind
                FROM master.Country
                WHERE id = @Id AND isDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();

        var raw = await connection.QueryFirstOrDefaultAsync<(Guid Id, string Name, string Code,string Tags, DateTime UpdatedAt, string UpdatedBy, int Act_Ind)>(sql, new { Id = id });

        if (raw.Equals(default)) return null!;

        return new CountryDetail
        {
            id = raw.Id,
            name = raw.Name,
            code = raw.Code,
            updated_At = raw.UpdatedAt,
            updated_By = raw.UpdatedBy,
            act_Ind = raw.Act_Ind
        };
    }

    public static async Task<PaginatedList<CountryList>> GetCountryListAsync(this MasterServiceContext _dbContext, Country dto, int index, int size)
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
        SELECT id, name, code,
        updatedAt, updatedBy, act_Ind
        FROM master.Country
        {whereClause}
        ORDER BY UpdatedAt DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM master.Country
        {whereClause};
        ";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<Country>();
        var totalCount = await multi.ReadFirstAsync<int>();

        var results = rawResults.Select(r => new CountryList
        {
            id = r.id,
            name = r.name,
            code = r.code,
            updated_At = r.updated_At,
            updated_By = r.updated_By,
            act_Ind = r.act_Ind
        }).ToList();

        return results.ToPaginatedList(index, size, totalCount);
    }

    // For Internal Use We will use Entity Framework
    public static async Task<Country> GetCountryByNameAsync(this MasterServiceContext _dbContext, string Name)
    {
        System.Linq.Expressions.Expression<Func<Country, bool>> predicate = a => a.name == Name;
        var countryDetails = await _dbContext.Country.AsNoTracking().FirstOrDefaultAsync(predicate);
        return countryDetails!;
    }

    ////public static async Task<Country> GetCountryByIdAsync(this MasterServiceContext _dbContext, Guid id)
    ////{
    ////    System.Linq.Expressions.Expression<Func<Country, bool>> predicate = a => a.Id == id;
    ////    var moduleDetails = await _dbContext.Country.AsNoTracking().FirstOrDefaultAsync(predicate);
    ////    return moduleDetails;
    ////}
}