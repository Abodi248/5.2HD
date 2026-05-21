namespace AboriginalArtGallery.Application.Artworks;

public class ArtworkDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public Guid ArtistId { get; set; }
    public string ArtistName { get; set; } = null!;
    public int? YearCreated { get; set; }
    public string Medium { get; set; } = null!;
    public decimal WidthCm { get; set; }
    public decimal HeightCm { get; set; }
    public Guid? ArtTypeId { get; set; }
    public string? Description { get; set; }
    public DateOnly? AcquisitionDate { get; set; }
    public decimal? AcquisitionPrice { get; set; }
    public bool IsOnDisplay { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
