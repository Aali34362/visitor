using System.Linq.Expressions;

namespace Visitor.Core.DesignPatterns.SpecificationPattern;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity) =>
        ToExpression().Compile().Invoke(entity);

    public Specification<T> And(Specification<T> other) =>
        new AndSpecification<T>(this, other);

    public Specification<T> Or(Specification<T> other) =>
        new OrSpecification<T>(this, other);

    public Specification<T> Not() =>
        new NotSpecification<T>(this);
}

internal class AndSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = _left.ToExpression();
        var rightExpr = _right.ToExpression();

        var param = leftExpr.Parameters.Single();
        var body = Expression.AndAlso(
            leftExpr.Body,
            Expression.Invoke(rightExpr, param));

        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}

internal class OrSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpr = _left.ToExpression();
        var rightExpr = _right.ToExpression();

        var param = leftExpr.Parameters.Single();
        var body = Expression.OrElse(
            leftExpr.Body,
            Expression.Invoke(rightExpr, param));

        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}

internal class NotSpecification<T> : Specification<T>
{
    private readonly Specification<T> _inner;

    public NotSpecification(Specification<T> inner) => _inner = inner;

    public override Expression<Func<T, bool>> ToExpression()
    {
        var innerExpr = _inner.ToExpression();
        var param = innerExpr.Parameters.Single();
        var body = Expression.Not(innerExpr.Body);

        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}

public static class SpecificationExtensions
{
    public static IQueryable<T> Where<T>(this IQueryable<T> query, Specification<T> specification) =>
        query.Where(specification.ToExpression());
}

// Example usage:
/*
public class InventoryItem
{
    public string Name { get; private set; }
    public int Quantity { get; private set; }
    public bool IsActive { get; private set; }
}

public class ActiveItemsSpec : Specification<InventoryItem>
    {
        public override Expression<Func<InventoryItem, bool>> ToExpression()
            => item => item.IsActive;
    }

    public class LowStockSpec : Specification<InventoryItem>
    {
        private readonly int _threshold;
        public LowStockSpec(int threshold) => _threshold = threshold;

        public override Expression<Func<InventoryItem, bool>> ToExpression()
            => item => item.Quantity < _threshold;
    }


public async Task<IReadOnlyList<InventoryItem>> GetItemsAsync(Specification<InventoryItem> spec)
{
    return await _dbContext.InventoryItems
        .Where(spec.ToExpression())
        .ToListAsync();
}

var spec = new ActiveItemsSpec().And(new LowStockSpec(10));
var lowStockActiveItems = await _inventoryRepo.GetItemsAsync(spec);

 */
