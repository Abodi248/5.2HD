using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Application.Artists;

public class UpdateArtistDto
{
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = null!;

    [MaxLength(200)]
    public string? KnownAs { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateOnly? DateOfDeath { get; set; }

    [MaxLength(5000)]
    public string? Biography { get; set; }

    public Guid? TribeId { get; set; }

    [Required]
    [MaxLength(200)]
    public string CountryOfOrigin { get; set; } = null!;
}
