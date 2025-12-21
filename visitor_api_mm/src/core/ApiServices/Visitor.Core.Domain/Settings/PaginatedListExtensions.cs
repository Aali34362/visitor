namespace Visitor.Core.Domain.Settings;

public static class PaginatedListExtensions
{
    public static PaginatedList<T> ToPaginatedList<T>(this IList<T> source, int pageNumber, int pageSize, int total)
    {
        int start = (pageNumber - 1) * pageSize + 1;
        int end = Math.Min(total, start + pageSize - 1);

        return new PaginatedList<T>
        {
            Items = source,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = total,
            Start = start,
            End = end
        };
    }
}

public sealed class PaginatedList<T>
{
    public IList<T> Items { get; set; } = null!;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int Start { get; set; }
    public int End { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

    public bool IsNullOrEmpty()
    {
        return Items == null || !Items.Any();
    }
}