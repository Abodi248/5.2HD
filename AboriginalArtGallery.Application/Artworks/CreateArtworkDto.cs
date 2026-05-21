using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Application.Artworks;

public class CreateArtworkDto
{
    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = null!;

    [Required]
    public Guid ArtistId { get; set; }

    public int? YearCreated { get; set; }

    [Required]
    [MaxLength(200)]
    public string Medium { get; set; } = null!;

    [Range(0.01, double.MaxValue, ErrorMessage = "Width must be greater than zero.")]
    public decimal WidthCm { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Height must be greater than zero.")]
    public decimal HeightCm { get; set; }

    public Guid? ArtTypeId { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    public DateOnly? AcquisitionDate { get; set; }

    [Range(0.0, double.MaxValue, ErrorMessage = "Acquisition price cannot be negative.")]
    public decimal? AcquisitionPrice { get; set; }

    public string? ImageUrl { get; set; }
}
