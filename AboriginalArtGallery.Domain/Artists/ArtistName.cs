using AboriginalArtGallery.Domain.Exceptions;

namespace AboriginalArtGallery.Domain.Artists;

public record ArtistName
{
    public string Value { get; }

    public ArtistName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Artist name cannot be empty.");

        var trimmed = value.Trim();

        if (trimmed.Length > 200)
            throw new DomainException("Artist name cannot exceed 200 characters.");

        Value = trimmed;
    }
}
