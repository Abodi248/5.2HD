namespace AboriginalArtGallery.Application.Artists;

public class ArtistSummaryDto
{
    public Guid ArtistId { get; set; }
    public string FullName { get; set; } = null!;
    public int ArtworkCount { get; set; }
    public int? EarliestYear { get; set; }
    public int? LatestYear { get; set; }
    public List<string> MediaUsed { get; set; } = new();
}
