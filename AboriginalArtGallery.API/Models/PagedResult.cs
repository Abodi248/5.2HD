namespace AboriginalArtGallery.API.Models;

/// <summary>A paginated result envelope returned by list endpoints.</summary>
public class PagedResult<T>
{
    /// <summary>The items in the current page.</summary>
    public IEnumerable<T> Items { get; set; } = null!;

    /// <summary>Total number of matching records across all pages.</summary>
    public int TotalCount { get; set; }

    /// <summary>Current page number (1-based).</summary>
    public int Page { get; set; }

    /// <summary>Maximum number of items returned per page.</summary>
    public int PageSize { get; set; }
}
