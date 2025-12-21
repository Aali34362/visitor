namespace Visitor.Module.IAM.Data.ContextExtension.IAM;

public static class UserContextExtension
{
    public static void CreateUser(this IAMServiceContext _dbContext, IdentityUser dto)
    {
        _dbContext.AddEntity(dto);
    }
    public static void UpdateUser(this IAMServiceContext _dbContext, IdentityUser dto)
    {
        _dbContext.UpdateEntity(dto);
    }
    public static void DeleteUser(this IAMServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<IdentityUser>(Id);
    }
    
    public static async Task<IdentityUserDetail> GetUserByIdAsync(this IAMServiceContext _dbContext, Guid id)
    {
        var sql = @"SELECT * FROM iam.IdentityUser u WHERE u.Id = @Id AND u.IsDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();

        var result = await connection.QueryFirstOrDefaultAsync<IdentityUserDetail>(sql, new { Id = id });

        return result!;
    }

    public static async Task<PaginatedList<IdentityUserList>> GetUserListAsync(this IAMServiceContext _dbContext, IdentityUser dto, int index, int size)
    {
        var whereClause = new StringBuilder("WHERE IsDeleted = FALSE");
        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(dto.UserName))
        {
            whereClause.Append(" AND LOWER(UserName) = @UserName");
            parameters.Add("UserName", dto.UserName.ToLower());
        }

        parameters.Add("Offset", (index - 1) * size);
        parameters.Add("PageSize", size);
        var sql = $@"
                      SELECT * FROM iam.IdentityUser
                      {whereClause}
                      ORDER BY UpdatedAt DESC
                      OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                      SELECT COUNT(1)
                      FROM iam.IdentityUser
                      {whereClause}";

        var connection = _dbContext.Database.GetDbConnection();
        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var rawResults = await multi.ReadAsync<IdentityUserList>();
        var totalCount = await multi.ReadFirstAsync<int>();

        return rawResults.ToList().ToPaginatedList(index, size, totalCount);
    }

    public static async Task<IdentityUser> GetUserByNameAsync(this IAMServiceContext _dbContext, string User_Nm)
    {
        System.Linq.Expressions.Expression<Func<IdentityUser, bool>> predicate = a => a.UserName == User_Nm;
        var userDetails = await _dbContext.IdentityUser.AsNoTracking().FirstOrDefaultAsync(predicate);
        return userDetails!;
    }

    public static async Task<IdentityUser> ValidateUserPasswordAsync(this IAMServiceContext _dbContext, string User_Nm, string password)
    {
        System.Linq.Expressions.Expression<Func<IdentityUser, bool>> predicate = a => a.UserName == User_Nm & a.PasswordHash == password;
        var userDetails = await _dbContext.IdentityUser.AsNoTracking().FirstOrDefaultAsync(predicate);
        return userDetails!;
    }

    public static async Task<bool> emailExistsAsync(this IAMServiceContext _dbContext, string email)
    {
        System.Linq.Expressions.Expression<Func<IdentityUser, bool>> predicate = a => a.Email == email;
        var exist = await _dbContext.IdentityUser.AsNoTracking().AnyAsync(predicate);
        return exist;
    }
}
