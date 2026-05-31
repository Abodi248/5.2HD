namespace AboriginalArtGallery.Application.ArtTypes;

public class ArtTypeDto
{
    /// <summary>The art type's unique identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>The name of the art type (e.g. "Dot Painting", "Bark Painting").</summary>
    public string Name { get; set; } = null!;

    /// <summary>The broad category this art type belongs to (e.g. "Painting", "Sculpture").</summary>
    public string Category { get; set; } = null!;

    /// <summary>An optional description of the art type.</summary>
    public string? Description { get; set; }

    /// <summary>Whether this art type record is active (not soft-deleted).</summary>
    public bool IsActive { get; set; }

    /// <summary>UTC timestamp when the record was created.</summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>UTC timestamp when the record was last updated.</summary>
    public DateTimeOffset UpdatedAt { get; set; }
}
