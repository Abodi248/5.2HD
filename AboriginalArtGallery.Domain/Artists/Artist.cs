namespace AboriginalArtGallery.Domain.Artists;

public class Artist
{
    public Guid Id { get; private set; }
    public ArtistName Name { get; private set; } = null!;
    public string? KnownAs { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public DateOnly? DateOfDeath { get; private set; }
    public Biography Bio { get; private set; } = null!;
    public Guid? TribeId { get; private set; }
    public string CountryOfOrigin { get; private set; } = null!;
    public bool IsVerified { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    private Artist() { }

    public static Artist Create(
        ArtistName name,
        string? knownAs,
        DateOnly? dob,
        DateOnly? dod,
        Biography bio,
        Guid? tribeId,
        string countryOfOrigin)
    {
        return new Artist
        {
            Id = Guid.NewGuid(),
            Name = name,
            KnownAs = knownAs,
            DateOfBirth = dob,
            DateOfDeath = dod,
            Bio = bio,
            TribeId = tribeId,
            CountryOfOrigin = countryOfOrigin,
            IsVerified = false,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Update(
        ArtistName name,
        string? knownAs,
        DateOnly? dob,
        DateOnly? dod,
        Biography bio,
        Guid? tribeId,
        string countryOfOrigin)
    {
        Name = name;
        KnownAs = knownAs;
        DateOfBirth = dob;
        DateOfDeath = dod;
        Bio = bio;
        TribeId = tribeId;
        CountryOfOrigin = countryOfOrigin;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Verify()
    {
        IsVerified = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
