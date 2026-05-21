using AboriginalArtGallery.Domain.Exceptions;

namespace AboriginalArtGallery.Domain.Artworks;

public record Medium
{
    public string Value { get; }

    public Medium(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Medium cannot be empty.");

        var trimmed = value.Trim();

        if (trimmed.Length > 200)
            throw new DomainException("Medium cannot exceed 200 characters.");

        Value = trimmed;
    }
}
