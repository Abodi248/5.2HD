namespace AboriginalArtGallery.Application.Artists;

public class ArtistDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? KnownAs { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? DateOfDeath { get; set; }
    public string? Biography { get; set; }
    public Guid? TribeId { get; set; }
    public string CountryOfOrigin { get; set; } = null!;
    public bool IsVerified { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
