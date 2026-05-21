namespace AboriginalArtGallery.Domain.Artworks;

public class Artwork
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = null!;
    public Guid ArtistId { get; private set; }
    public int? YearCreated { get; private set; }
    public Medium ArtMedium { get; private set; } = null!;
    public Dimensions Size { get; private set; } = null!;
    public Guid? ArtTypeId { get; private set; }
    public string? Description { get; private set; }
    public AcquisitionInfo Acquisition { get; private set; } = null!;
    public bool IsOnDisplay { get; private set; }
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
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
