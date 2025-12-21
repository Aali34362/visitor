namespace Visitor.Module.IAM.Data.ContextExtension.IAM;

public static class UserLoginLoginContextExtension
{
    public static void CreateUserLogin(this IAMServiceContext _dbContext, IdentityUserLogin dto)
    {
        _dbContext.AddEntity(dto);
    }
    public static void UpdateUserLogin(this IAMServiceContext _dbContext, IdentityUserLogin dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static async Task<PaginatedList<IdentityUserLoginList>> GetUserLoginListAsync(this IAMServiceContext _dbContext, IdentityUserLogin dto, int index, int size)
    {
        var whereClause = new StringBuilder("WHERE ul.IsDeleted = FALSE AND u.IsDeleted = FALSE");
        var parameters = new DynamicParameters();

        if (dto.User_Id != Guid.Empty)
        {
            whereClause.Append(" AND ul.User_Id = @User_Id");
            parameters.Add("User_Id", dto.User_Id);
        }

        if (dto.Session_Id != Guid.Empty)
        {
            whereClause.Append(" AND ul.Session_Id = @Session_Id");
            parameters.Add("Session_Id", dto.Session_Id);
        }

        parameters.Add("Offset", (index - 1) * size);
        parameters.Add("PageSize", size);
        var sql = $@"
                      SELECT ul.User_Id, ul.Session_Id, ul.Login_Source_Sytem, ul.Login_Source_Sytem_Ip, ul.Login_Date, ul.Logout_Date,
                      u.UserName 
                      FROM iam.IdentityUserLogin ul
                      JOIN iam.IdentityUser u ON ul.User_Id = u.Id   
                      {whereClause}
                      ORDER BY ul.UpdatedAt DESC
                      OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                      SELECT COUNT(1)
                      FROM iam.IdentityUserLogin ul
                      JOIN iam.IdentityUser u ON ul.User_Id = u.Id
                      {whereClause}";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<IdentityUserLoginList>();
        var totalCount = await multi.ReadFirstAsync<int>();

        return rawResults.ToList().ToPaginatedList(index, size, totalCount);
    }

    public static async Task<IdentityUserLogin> GetUserLoginByIdAsync(this IAMServiceContext _dbContext, Guid id)
    {
        System.Linq.Expressions.Expression<Func<IdentityUserLogin, bool>> predicate = a => a.Id == id;
        var userLoginDetails = await _dbContext.IdentityUserLogin.AsNoTracking().FirstOrDefaultAsync(predicate);
        return userLoginDetails!;
    }
}
