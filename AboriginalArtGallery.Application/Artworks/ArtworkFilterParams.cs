using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Application.Artworks;

public class ArtworkFilterParams
{
    public Guid? ArtistId { get; set; }
    public bool? OnDisplay { get; set; }
    public string? Medium { get; set; }
    public int? YearFrom { get; set; }
    public int? YearTo { get; set; }
    public Guid? ArtTypeId { get; set; }

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 20;
}
