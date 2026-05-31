using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Application.Tribes;

public class CreateTribeDto
{
    /// <summary>The name of the tribe (required, max 200 characters).</summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    /// <summary>The geographic region associated with the tribe (required, max 200 characters).</summary>
    [Required]
    [MaxLength(200)]
    public string Region { get; set; } = null!;

    /// <summary>The language group the tribe belongs to (optional, max 200 characters).</summary>
    [MaxLength(200)]
    public string? LanguageGroup { get; set; }

    /// <summary>An optional description of the tribe's cultural background (max 2000 characters).</summary>
    [MaxLength(2000)]
    public string? Description { get; set; }
}
