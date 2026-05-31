using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Application.ArtTypes;

public class CreateArtTypeDto
{
    /// <summary>The name of the art type (required, max 200 characters).</summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>The broad category this art type belongs to (required, max 200 characters).</summary>
    [Required]
    [MaxLength(200)]
    public string Category { get; set; } = null!;

    /// <summary>An optional description of the art type (max 2000 characters).</summary>
    [MaxLength(2000)]
    public string? Description { get; set; }
}
