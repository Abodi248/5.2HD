namespace AboriginalArtGallery.Application.Tribes;

public class TribeDto
{
    /// <summary>The tribe's unique identifier.</summary>
    public Guid Id { get; set; }

    /// <summary>The name of the tribe.</summary>
    public string Name { get; set; } = null!;

    /// <summary>The geographic region the tribe is associated with.</summary>
    public string Region { get; set; } = null!;

    /// <summary>The language group the tribe belongs to, if known.</summary>
    public string? LanguageGroup { get; set; }

    /// <summary>An optional description of the tribe's cultural background.</summary>
    public string? Description { get; set; }

    /// <summary>Whether this tribe record is active (not soft-deleted).</summary>
    public bool IsActive { get; set; }

    /// <summary>UTC timestamp when the record was created.</summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>UTC timestamp when the record was last updated.</summary>
    public DateTimeOffset UpdatedAt { get; set; }
}
