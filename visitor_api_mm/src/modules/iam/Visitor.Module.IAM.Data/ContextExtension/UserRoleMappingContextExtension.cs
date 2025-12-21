namespace Visitor.Module.IAM.Data.ContextExtension.IAM;

public static class UserRoleContextExtension
{
    public static void CreateUserRole(this IAMServiceContext _dbContext, IdentityUserRoleMapping dto)
    {
        _dbContext.AddEntity(dto);
    }

    public static void UpdateUserRole(this IAMServiceContext _dbContext, IdentityUserRoleMapping dto)
    {
        _dbContext.UpdateEntity(dto);
    }

    public static void DeleteUserRole(this IAMServiceContext _dbContext, Guid Id)
    {
        _dbContext.DeleteEntity<IdentityUserRoleMapping>(Id);
    }

    public static async Task<IdentityUserRoleMappingDetail> GetUserRoleByIdAsync(this IAMServiceContext _dbContext, Guid id)
    {
        var sql = @"
        SELECT 
            ur.Id,
            u.UserName AS User_Nm, 
            r.Name AS Role_Nm,
            ur.UpdatedAt, 
            ur.UpdatedBy, 
            ur.Act_Ind
        FROM iam.IdentityUserRoleMapping ur
        JOIN iam.IdentityUser u ON ur.User_Id = u.Id
        JOIN iam.IdentityRole r ON ur.Role_Id = r.Id
        WHERE ur.Id = @Id 
          AND ur.IsDeleted = FALSE  
          AND u.IsDeleted = FALSE  
          AND r.IsDeleted = FALSE";

        var connection = _dbContext.Database.GetDbConnection();

        try
        {
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            var result = await connection.QueryFirstOrDefaultAsync<IdentityUserRoleMappingDetail>(sql, new { Id = id });
            return result!;
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                await connection.CloseAsync();
        }
    }

    public static async Task<PaginatedList<IdentityUserRoleMappingList>> GetUserRoleListAsync(
    this IAMServiceContext _dbContext,
    IdentityUserRoleMapping dto,
    int index,
    int size)
    {
        var whereClause = new StringBuilder("WHERE ur.IsDeleted = FALSE AND u.IsDeleted = FALSE AND r.IsDeleted = FALSE");
        var parameters = new DynamicParameters();

        if (dto.User_Id != Guid.Empty)
        {
            whereClause.Append(" AND ur.User_Id = @UserId");
            parameters.Add("UserId", dto.User_Id);
        }

        if (dto.Role_Id != Guid.Empty)
        {
            whereClause.Append(" AND ur.Role_Id = @RoleId");
            parameters.Add("RoleId", dto.Role_Id);
        }

        parameters.Add("Offset", (index - 1) * size);
        parameters.Add("PageSize", size);

        var sql = $@"
        SELECT 
            ur.Id,
            u.UserName AS User_Nm, 
            r.Name AS Role_Nm,
            ur.UpdatedAt, 
            ur.UpdatedBy, 
            ur.Act_Ind
        FROM iam.IdentityUserRoleMapping ur
        JOIN iam.IdentityUser u ON ur.User_Id = u.Id
        JOIN iam.IdentityRole r ON ur.Role_Id = r.Id
        {whereClause}
        ORDER BY ur.UpdatedAt DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(1)
        FROM iam.IdentityUserRoleMapping ur
        JOIN iam.IdentityUser u ON ur.User_Id = u.Id
        JOIN iam.IdentityRole r ON ur.Role_Id = r.Id
        {whereClause};";

        var connection = _dbContext.Database.GetDbConnection();

        try
        {
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using var multi = await connection.QueryMultipleAsync(sql, parameters);

            var rawResults = await multi.ReadAsync<IdentityUserRoleMappingList>();
            var totalCount = await multi.ReadFirstAsync<int>();

            return rawResults.ToList().ToPaginatedList(index, size, totalCount);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                await connection.CloseAsync();
        }
    }


    public static async Task<List<IdentityUserRoleMappingList>> GetUserRoleAsync(this IAMServiceContext _dbContext, Guid id)
    {
        var baseQuery = _dbContext.IdentityUserRoleMapping.AsNoTracking().AsQueryable();

        baseQuery = baseQuery.Where(x => x.Id == id);

        var joinedQuery = from ur in baseQuery
                          join u in _dbContext.IdentityUser.AsNoTracking() on ur.User_Id equals u.Id
                          join r in _dbContext.IdentityRole.AsNoTracking() on ur.Role_Id equals r.Id
                          select new { ur, u, r };

        var result = await joinedQuery
            .Select(x => new IdentityUserRoleMappingList
            {
                Id = x.ur.Id,
                User_Nm = x.u.UserName,
                Role_Nm = x.r.Name,
                UpdatedAt = x.ur.UpdatedAt,
                UpdatedBy = x.ur.UpdatedBy,
                Act_Ind = x.ur.Act_Ind
            })
            .ToListAsync();
        return result;
    }


    public static async Task<bool> IsUserRoleMappingExistsAsync(this IAMServiceContext _dbContext, Guid user_id, Guid role_id)
    {
        System.Linq.Expressions.Expression<Func<IdentityUserRoleMapping, bool>> predicate = a => a.User_Id == user_id & a.Role_Id == role_id;
        var exist = await _dbContext.IdentityUserRoleMapping.AsNoTracking().AnyAsync(predicate);
        return exist;
    }
}
