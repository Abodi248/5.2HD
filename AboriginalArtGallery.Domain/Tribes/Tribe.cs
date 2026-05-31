using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AboriginalArtGallery.Domain.Tribes;

public class Tribe
{
    [BsonId]
    [BsonElement("_id")]
    public Guid Id { get; private set; }

    [BsonElement("name")]
    public string Name { get; private set; } = null!;

    [BsonElement("region")]
    public string Region { get; private set; } = null!;

    [BsonElement("language_group")]
    [BsonIgnoreIfNull]
    public string? LanguageGroup { get; private set; }

    [BsonElement("description")]
    [BsonIgnoreIfNull]
    public string? Description { get; private set; }

    [BsonElement("is_active")]
    public bool IsActive { get; private set; }

    [BsonElement("created_at")]
    public DateTimeOffset CreatedAt { get; private set; }

    [BsonElement("updated_at")]
    public DateTimeOffset UpdatedAt { get; private set; }

    private Tribe() { }

    public static Tribe Create(string name, string region, string? languageGroup, string? description)
    {
        return new Tribe
        {
            Id = Guid.NewGuid(),
            Name = name,
            Region = region,
            LanguageGroup = languageGroup,
            Description = description,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Update(string name, string region, string? languageGroup, string? description)
    {
        Name = name;
        Region = region;
        LanguageGroup = languageGroup;
        Description = description;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
