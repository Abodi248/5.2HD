using AboriginalArtGallery.Domain.Exceptions;

namespace AboriginalArtGallery.Domain.Artists;

public record Biography
{
    public string? Value { get; }

    public Biography(string? value)
    {
        if (value is not null && value.Length > 5000)
            throw new DomainException("Biography cannot exceed 5000 characters.");

        Value = value;
    }
}
