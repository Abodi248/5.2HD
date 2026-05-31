using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AboriginalArtGallery.Domain.ArtTypes;

public class ArtType
{
    [BsonId]
    [BsonElement("_id")]
    public Guid Id { get; private set; }

    [BsonElement("name")]
    public string Name { get; private set; } = null!;

    [BsonElement("category")]
    public string Category { get; private set; } = null!;

    [BsonElement("description")]
    [BsonIgnoreIfNull]
    public string? Description { get; private set; }

    [BsonElement("is_active")]
    public bool IsActive { get; private set; }

    [BsonElement("created_at")]
    public DateTimeOffset CreatedAt { get; private set; }

    [BsonElement("updated_at")]
    public DateTimeOffset UpdatedAt { get; private set; }

    private ArtType() { }

    public static ArtType Create(string name, string category, string? description)
    {
        return new ArtType
        {
            Id = Guid.NewGuid(),
            Name = name,
            Category = category,
            Description = description,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Update(string name, string category, string? description)
    {
        Name = name;
        Category = category;
        Description = description;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
