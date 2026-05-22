using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AboriginalArtGallery.Domain.Artworks;

public class Artwork
{
    [BsonId]
    [BsonElement("_id")]
    public Guid Id { get; private set; }

    [BsonElement("title")]
    public string Title { get; private set; } = null!;

    [BsonElement("artist_id")]
    public Guid ArtistId { get; private set; }

    [BsonElement("year_created")]
    [BsonIgnoreIfNull]
    public int? YearCreated { get; private set; }

    [BsonElement("medium")]
    public Medium ArtMedium { get; private set; } = null!;

    [BsonElement("dimensions")]
    public Dimensions Size { get; private set; } = null!;

    [BsonElement("art_type_id")]
    [BsonIgnoreIfNull]
    public Guid? ArtTypeId { get; private set; }

    [BsonElement("description")]
    [BsonIgnoreIfNull]
    public string? Description { get; private set; }

    [BsonElement("acquisition")]
    public AcquisitionInfo Acquisition { get; private set; } = null!;

    [BsonElement("is_on_display")]
    public bool IsOnDisplay { get; private set; }

    [BsonElement("image_url")]
    [BsonIgnoreIfNull]
    public string? ImageUrl { get; private set; }

    [BsonElement("is_active")]
    public bool IsActive { get; private set; }

    [BsonElement("created_at")]
    public DateTimeOffset CreatedAt { get; private set; }

    [BsonElement("updated_at")]
    public DateTimeOffset UpdatedAt { get; private set; }

    private Artwork() { }

    public static Artwork Create(
        string title,
        Guid artistId,
        int? yearCreated,
        Medium medium,
        Dimensions dimensions,
        Guid? artTypeId,
        string? description,
        AcquisitionInfo acquisition,
        string? imageUrl)
    {
        return new Artwork
        {
            Id = Guid.NewGuid(),
            Title = title,
            ArtistId = artistId,
            YearCreated = yearCreated,
            ArtMedium = medium,
            Size = dimensions,
            ArtTypeId = artTypeId,
            Description = description,
            Acquisition = acquisition,
            IsOnDisplay = false,
            ImageUrl = imageUrl,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Update(
        string title,
        int? yearCreated,
        Medium medium,
        Dimensions dimensions,
        Guid? artTypeId,
        string? description,
        AcquisitionInfo acquisition,
        string? imageUrl)
    {
        Title = title;
        YearCreated = yearCreated;
        ArtMedium = medium;
        Size = dimensions;
        ArtTypeId = artTypeId;
        Description = description;
        Acquisition = acquisition;
        ImageUrl = imageUrl;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SetDisplayStatus(bool isOnDisplay)
    {
        IsOnDisplay = isOnDisplay;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
