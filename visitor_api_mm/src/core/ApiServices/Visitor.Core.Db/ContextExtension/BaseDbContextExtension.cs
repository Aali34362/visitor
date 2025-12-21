namespace Visitor.Core.Db.ContextExtension;

public static class BaseDbContextExtension
{
    #region Read
    public static T GetEntity<T>(this BaseDbContext context, Guid id) where T : class
    {
        return context.Set<T>().Find(id)!;
    }

    public static T GetEntity<T>(this BaseDbContext context, string code) where T : class
    {
        return context.Set<T>().Find(code)!;
    }

    public static List<T> GetEntity<T>(this BaseDbContext context, Func<T, bool> predicate) where T : class
    {
        return context.Set<T>().Where(predicate).ToList();
    }

    public static T GetFirstEntity<T>(this BaseDbContext context, Func<T, bool> predicate) where T : class
    {
        return context.Set<T>().FirstOrDefault(predicate)!;
    }

    public static List<T> GetList<T>(this BaseDbContext context) where T : class
    {
        return context.Set<T>().ToList();
    }

    public static Task<List<T>> GeListAsync<T>(this BaseDbContext context) where T : class
    {
        return context.Set<T>().ToListAsync();
    }

    public static string GetEntityMax<T>(this BaseDbContext context, Func<T, string> max, string prefix, string pattern) where T : class
    {
        string code = context.Set<T>().Max(max)!;
        if (string.IsNullOrEmpty(code))
            code = prefix + pattern;

        return prefix + (Convert.ToInt32(code.Replace(prefix, string.Empty)) + 1).ToString().PadLeft(pattern.Length, '0');
    }

    public static string GetEntityMax<T>(this BaseDbContext context, Func<T, string> max, string pattern) where T : class
    {
        string code = context.Set<T>().Max(max)!;
        if (string.IsNullOrEmpty(code))
            code = pattern;

        return (Convert.ToInt32(code) + 1).ToString().PadLeft(pattern.Length, '0');
    }

    public static string GetEntityMax<T>(this BaseDbContext context, Func<T, bool> where, Func<T, string> max, string prefix, string pattern) where T : class
    {
        string code = context.Set<T>().Where(where).Max(max)!;
        if (string.IsNullOrEmpty(code))
            code = prefix + pattern;

        return prefix + (Convert.ToInt32(code.Replace(prefix, string.Empty)) + 1).ToString().PadLeft(pattern.Length, '0');
    }

    public static string GetEntityMax<T>(this BaseDbContext context, Func<T, bool> where, Func<T, string> max, string pattern) where T : class
    {
        string code = context.Set<T>().Where(where).Max(max)!;
        if (string.IsNullOrEmpty(code))
            code = pattern;

        return (Convert.ToInt32(code) + 1).ToString().PadLeft(pattern.Length, '0');
    }
    #endregion

    #region Write
    public static void AddEntity<T>(this BaseDbContext context, T entity, short actvInd = 1, bool isDeleted = false) where T : class
    {
        context.Set<T>().Add(entity);
    }

    public static void AddRangeEntity<T>(this BaseDbContext context, List<T> entities, short actvInd = 1, bool isDeleted = false) where T : class
    {
        context.Set<T>().AddRange(entities);
    }

    public static void UpdateEntity<T>(this BaseDbContext context, T entity, bool audit = false, params string[] properties) where T : class
    {
        context.Entry(entity).State = EntityState.Modified;
    }

    public static void UpdateRangeEntity<T>(this BaseDbContext context, IList<T> entities, bool audit = false, params string[] properties) where T : class
    {
        foreach (var entity in entities)
        {
            context.Entry(entity).State = EntityState.Modified;

            foreach (string property in properties)
            {
                context.Entry(entity).Property(property).IsModified = false;
            }
        }
        context.UpdateRange(entities);
    }

    public static T DeleteEntity<T>(this BaseDbContext context, Guid id, short actvInd = 0, bool isDeleted = true, bool audit = false) where T : class
    {
        var entity = context.GetEntity<T>(id);
        context.Entry(entity).State = EntityState.Deleted;
        return entity;
    }

    public static void DeleteRangeEntity<T>(this BaseDbContext context, List<T> entities, short actvInd = 0, bool isDeleted = true) where T : class
    {
        foreach (var entity in entities)
            context.Entry(entity).State = EntityState.Deleted;
        context.UpdateRange(entities);
    }

    private static void MarkSoftDeleted<T>(T entity, short actvInd = 0, bool isDeleted = true) where T : class
    {
        var type = typeof(T);

        type.GetProperty("Act_Ind")?.SetValue(entity, actvInd);
        type.GetProperty("IsDeleted")?.SetValue(entity, isDeleted);
        type.GetProperty("UpdatedAt")?.SetValue(entity, DateTime.UtcNow);
        type.GetProperty("UpdatedBy")?.SetValue(entity, "System"); // Or pull from user context
        type.GetProperty("Version")?.SetValue(entity, ((int?)type.GetProperty("Version")?.GetValue(entity) ?? 1) + 1);
    }
    #endregion
    public static Dictionary<string, string> ConvertTags(string Tags)
    {
        return string.IsNullOrEmpty(Tags)
                ? new Dictionary<string, string>()
                : JsonSerializer.Deserialize<Dictionary<string, string>>(Tags);
    }

}