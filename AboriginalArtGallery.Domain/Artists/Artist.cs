using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AboriginalArtGallery.Domain.Artists;

public class Artist
{
    [BsonId]
    [BsonElement("_id")]
    public Guid Id { get; private set; }

    [BsonElement("full_name")]
    public ArtistName Name { get; private set; } = null!;

    [BsonElement("known_as")]
    [BsonIgnoreIfNull]
    public string? KnownAs { get; private set; }

    [BsonElement("date_of_birth")]
    [BsonIgnoreIfNull]
    public DateOnly? DateOfBirth { get; private set; }

    [BsonElement("date_of_death")]
    [BsonIgnoreIfNull]
    public DateOnly? DateOfDeath { get; private set; }

    [BsonElement("biography")]
    public Biography Bio { get; private set; } = null!;

    [BsonElement("tribe_id")]
    [BsonIgnoreIfNull]
    public Guid? TribeId { get; private set; }

    [BsonElement("country_of_origin")]
    public string CountryOfOrigin { get; private set; } = null!;

    [BsonElement("is_verified")]
    public bool IsVerified { get; private set; }

    [BsonElement("is_active")]
    public bool IsActive { get; private set; }

    [BsonElement("created_at")]
    public DateTimeOffset CreatedAt { get; private set; }

    [BsonElement("updated_at")]
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
