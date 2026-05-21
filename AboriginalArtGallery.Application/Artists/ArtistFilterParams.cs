using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Application.Artists;

public class ArtistFilterParams
{
    public string? Search { get; set; }
    public Guid? TribeId { get; set; }
    public bool? IsVerified { get; set; }

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 20;
}
