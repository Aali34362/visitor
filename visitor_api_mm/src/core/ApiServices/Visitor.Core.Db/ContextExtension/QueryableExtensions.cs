using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Visitor.Core.Db.ContextExtension;

public static class QueryableExtensions
{
    // -------------------------
    // Existing helpers (yours)
    // -------------------------
    public static IQueryable<T> WhereContainsIgnoreCase<T>(this IQueryable<T> source,
        Expression<Func<T, string>> property, string value)
    {
        if (string.IsNullOrEmpty(value)) return source;

        var notNull = Expression.NotEqual(property.Body, Expression.Constant(null, typeof(string)));
        var toLower = Expression.Call(property.Body, nameof(string.ToLowerInvariant), Type.EmptyTypes);
        var valueLower = Expression.Constant(value.ToLowerInvariant());
        var contains = Expression.Call(toLower, nameof(string.Contains), Type.EmptyTypes, valueLower);
        var combined = Expression.AndAlso(notNull, contains);

        return source.Where(Expression.Lambda<Func<T, bool>>(combined, property.Parameters));
    }

    public static IQueryable<T> WhereEqualsIgnoreCase<T>(this IQueryable<T> source,
        Expression<Func<T, string>> property, string value)
    {
        if (string.IsNullOrEmpty(value)) return source;

        var notNull = Expression.NotEqual(property.Body, Expression.Constant(null, typeof(string)));
        var toLower = Expression.Call(property.Body, nameof(string.ToLowerInvariant), Type.EmptyTypes);
        var valueLower = Expression.Constant(value.ToLowerInvariant());
        var equals = Expression.Equal(toLower, valueLower);
        var combined = Expression.AndAlso(notNull, equals);

        return source.Where(Expression.Lambda<Func<T, bool>>(combined, property.Parameters));
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition,
        Expression<Func<T, bool>> predicate)
        => condition ? source.Where(predicate) : source;

    // ---------------------------------
    // 1) IN / NOT IN for collections
    // ---------------------------------
    public static IQueryable<T> WhereIn<T, TKey>(this IQueryable<T> source,
        Expression<Func<T, TKey>> keySelector, IEnumerable<TKey> values)
    {
        var list = values?.ToList();
        if (list is null || list.Count == 0) return source;

        var param = keySelector.Parameters.Single();
        var body = Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), new[] { typeof(TKey) },
            Expression.Constant(list), keySelector.Body);
        return source.Where(Expression.Lambda<Func<T, bool>>(body, param));
    }

    public static IQueryable<T> WhereNotIn<T, TKey>(this IQueryable<T> source,
        Expression<Func<T, TKey>> keySelector, IEnumerable<TKey> values)
    {
        var list = values?.ToList();
        if (list is null || list.Count == 0) return source;

        var param = keySelector.Parameters.Single();
        var contains = Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), new[] { typeof(TKey) },
            Expression.Constant(list), keySelector.Body);
        var notContains = Expression.Not(contains);
        return source.Where(Expression.Lambda<Func<T, bool>>(notContains, param));
    }

    // ---------------------------------
    // 2) Between (for ranges)
    // ---------------------------------
    public static IQueryable<T> WhereBetween<T, TKey>(this IQueryable<T> source,
        Expression<Func<T, TKey>> selector, TKey? from, TKey? to, bool inclusiveEnd = true)
        where TKey : struct, IComparable<TKey>
    {
        if (from is null && to is null) return source;

        var p = selector.Parameters.Single();
        Expression body = null;

        if (from is not null)
        {
            var ge = Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(from.Value));
            body = ge;
        }
        if (to is not null)
        {
            var endOp = inclusiveEnd
                ? Expression.LessThanOrEqual(selector.Body, Expression.Constant(to.Value))
                : Expression.LessThan(selector.Body, Expression.Constant(to.Value));
            body = body is null ? endOp : Expression.AndAlso(body, endOp);
        }

        return source.Where(Expression.Lambda<Func<T, bool>>(body!, p));
    }

    // ---------------------------------
    // 3) Dynamic OrderBy by property name
    // ---------------------------------
    public static IOrderedQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string propertyName, bool desc = false)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return (IOrderedQueryable<T>)source.OrderByDescending(x => EF.Property<DateTime?>(x!, "UpdatedAt"));

        var param = Expression.Parameter(typeof(T), "x");
        var body = Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(object) }, param, Expression.Constant(propertyName));
        var converted = Expression.Convert(body, typeof(object));
        var lambda = Expression.Lambda<Func<T, object>>(converted, param);
        return desc ? source.OrderByDescending(lambda) : source.OrderBy(lambda);
    }

    public static IOrderedQueryable<T> ThenByProperty<T>(this IOrderedQueryable<T> source, string propertyName, bool desc = false)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var body = Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(object) }, param, Expression.Constant(propertyName));
        var converted = Expression.Convert(body, typeof(object));
        var lambda = Expression.Lambda<Func<T, object>>(converted, param);
        return desc ? source.ThenByDescending(lambda) : source.ThenBy(lambda);
    }

    // ---------------------------------
    // 4) Paging (provider-agnostic)
    // ---------------------------------
    public static async Task<(List<T> Items, int Total)> ToPageAsync<T>(this IQueryable<T> source,
        int pageIndex, int pageSize, CancellationToken ct = default)
    {
        var total = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    // ---------------------------------
    // 5) EF toggles (nice for pipelines)
    // ---------------------------------
    public static IQueryable<T> AsNoTrackingIf<T>(this IQueryable<T> source, bool condition)
        => condition ? source.AsNoTrackingIf(condition) : source;

    public static IQueryable<T> AsSplitQueryIf<T>(this IQueryable<T> source, bool condition)
        => condition ? source.AsSplitQueryIf(condition) : source;

    public static IQueryable<T> IgnoreQueryFiltersIf<T>(this IQueryable<T> source, bool condition)
        => condition ? source.IgnoreQueryFiltersIf(condition) : source;

    public static IQueryable<T> IncludeIf<T, TProperty>(this IQueryable<T> source,
        bool condition, Expression<Func<T, TProperty>> navigationPropertyPath) where T : class
        => condition ? source.Include(navigationPropertyPath) : source;

    // ---------------------------------
    // 6) Tag queries with caller (debugging)
    // ---------------------------------
    public static IQueryable<T> TagWithCallerMemberName<T>(this IQueryable<T> source,
        [CallerMemberName] string member = "")
        => source.TagWith($"Query: {member}");

    // ---------------------------------
    // 7) LeftJoin & InnerJoin wrappers
    //     (thin sugar over LINQ Join/GroupJoin)
    // ---------------------------------
    public static IQueryable<TResult> InnerJoin<TOuter, TInner, TKey, TResult>(
        this IQueryable<TOuter> outer,
        IQueryable<TInner> inner,
        Expression<Func<TOuter, TKey>> outerKeySelector,
        Expression<Func<TInner, TKey>> innerKeySelector,
        Expression<Func<TOuter, TInner, TResult>> resultSelector)
        => outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector);

    public static IQueryable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
        this IQueryable<TOuter> outer,
        IQueryable<TInner> inner,
        Expression<Func<TOuter, TKey>> outerKeySelector,
        Expression<Func<TInner, TKey>> innerKeySelector,
        Expression<Func<TOuter, TInner, TResult>> resultSelector)
    {
        // outer GroupJoin inner => flatten with DefaultIfEmpty
        return outer
            .GroupJoin(inner, outerKeySelector, innerKeySelector, (o, inners) => new { o, inners })
            .SelectMany(x => x.inners.DefaultIfEmpty(), (x, i) => new { x.o, i })
            .Select(x => resultSelector.Compose(x.o, x.i));
    }

    // helper to apply a lambda with two args in Select
    private static TResult Compose<TOuter, TInner, TResult>(this Expression<Func<TOuter, TInner, TResult>> projector,
        TOuter outer, TInner inner) => projector.Compile().Invoke(outer, inner); // compile-only used post-translation

    // ---------------------------------
    // 8) DistinctBy (for older frameworks)
    // ---------------------------------
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector)
    {
        var set = new HashSet<TKey>();
        foreach (var element in source)
            if (set.Add(keySelector(element)))
                yield return element;
    }

    // ---------------------------------
    // 9) Predicate Builder (And/Or chains)
    // ---------------------------------
    public static Expression<Func<T, bool>> True<T>() => _ => true;
    public static Expression<Func<T, bool>> False<T>() => _ => false;

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var body = Expression.AndAlso(
            Expression.Invoke(left, param),
            Expression.Invoke(right, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var body = Expression.OrElse(
            Expression.Invoke(left, param),
            Expression.Invoke(right, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }

    // ---------------------------------
    // 10) Postgres ILIKE (server-side)
    //     Requires Npgsql.EntityFrameworkCore.PostgreSQL
    // ---------------------------------
    ////public static IQueryable<T> WhereILike<T>(this IQueryable<T> source,
    ////    Expression<Func<T, string?>> property, string pattern)
    ////{
    ////    if (string.IsNullOrWhiteSpace(pattern)) return source;

    ////    // Build: EF.Functions.ILike(property, pattern)
    ////    var functions = Expression.Property(null, typeof(EF), nameof(EF.Functions)); // EF.Functions
    ////    var call = Expression.Call(
    ////        typeof(Npgsql.EntityFrameworkCore.PostgreSQL.NpgsqlDbFunctionsExtensions),
    ////        nameof(Npgsql.EntityFrameworkCore.PostgreSQL.NpgsqlDbFunctionsExtensions.ILike),
    ////        Type.EmptyTypes,
    ////        functions,
    ////        property.Body,
    ////        Expression.Constant(pattern));

    ////    var lambda = Expression.Lambda<Func<T, bool>>(call, property.Parameters);
    ////    return source.Where(lambda);
    ////}
}

