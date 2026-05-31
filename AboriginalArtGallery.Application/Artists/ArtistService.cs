using AboriginalArtGallery.Application.Artworks;
using AboriginalArtGallery.Domain.Artists;
using AboriginalArtGallery.Domain.Artworks;
using Microsoft.Extensions.Logging;

namespace AboriginalArtGallery.Application.Artists;

public class ArtistService
{
    private readonly IArtistRepository _repository;
    private readonly ILogger<ArtistService> _logger;

    public ArtistService(IArtistRepository repository, ILogger<ArtistService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ArtistDto> GetByIdAsync(Guid id)
    {
        var artist = await _repository.GetByIdAsync(id);
        if (artist is null)
        {
            _logger.LogWarning("Artist {Id} not found", id);
            throw new KeyNotFoundException($"Artist {id} not found.");
        }
        _logger.LogInformation("Retrieved artist {Id}", id);
        return MapToDto(artist);
    }

    public async Task<(IEnumerable<ArtistDto> Items, int TotalCount)> GetAllAsync(ArtistFilterParams filters)
    {
        var (items, total) = await _repository.GetAllAsync(filters);
        var dtoItems = items.Select(MapToDto).ToList();
        _logger.LogInformation("Retrieved {Count} artists (total: {Total})", dtoItems.Count, total);
        return (dtoItems, total);
    }

    public async Task<IEnumerable<ArtworkDto>> GetArtworksByArtistIdAsync(Guid artistId)
    {
        var artworks = await _repository.GetArtworksByArtistIdAsync(artistId);
        var dtoItems = artworks.Select(MapArtworkToDto).ToList();
        _logger.LogInformation("Retrieved {Count} artworks for artist {ArtistId}", dtoItems.Count, artistId);
        return dtoItems;
    }

    public async Task<ArtistSummaryDto> GetSummaryAsync(Guid artistId)
    {
        var summary = await _repository.GetSummaryAsync(artistId);
        if (summary is null)
        {
            _logger.LogWarning("Summary not found for artist {ArtistId}", artistId);
            throw new KeyNotFoundException($"Artist {artistId} not found.");
        }
        _logger.LogInformation("Retrieved summary for artist {ArtistId}", artistId);
        return summary;
    }

    public async Task<ArtistDto> CreateAsync(CreateArtistDto dto)
    {
        var name = new ArtistName(dto.FullName);
        var bio = new Biography(dto.Biography);
        var artist = Artist.Create(name, dto.KnownAs, dto.DateOfBirth, dto.DateOfDeath, bio, dto.TribeId, dto.CountryOfOrigin);
        await _repository.AddAsync(artist);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Created artist {Id}", artist.Id);
        return MapToDto(artist);
    }

    public async Task<ArtistDto> UpdateAsync(Guid id, UpdateArtistDto dto)
    {
        var artist = await _repository.GetByIdAsync(id);
        if (artist is null)
        {
            _logger.LogWarning("Artist {Id} not found for update", id);
            throw new KeyNotFoundException($"Artist {id} not found.");
        }
        var name = new ArtistName(dto.FullName);
        var bio = new Biography(dto.Biography);
        artist.Update(name, dto.KnownAs, dto.DateOfBirth, dto.DateOfDeath, bio, dto.TribeId, dto.CountryOfOrigin);
        await _repository.UpdateAsync(artist);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Updated artist {Id}", id);
        return MapToDto(artist);
    }

    public async Task DeleteAsync(Guid id)
    {
        var artist = await _repository.GetByIdAsync(id);
        if (artist is null)
        {
            _logger.LogWarning("Artist {Id} not found for deletion", id);
            throw new KeyNotFoundException($"Artist {id} not found.");
        }
        artist.Deactivate();
        await _repository.UpdateAsync(artist);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Deactivated artist {Id}", id);
    }

    private static ArtistDto MapToDto(Artist artist) => new()
    {
        Id = artist.Id,
        FullName = artist.Name.Value,
        KnownAs = artist.KnownAs,
        DateOfBirth = artist.DateOfBirth,
        DateOfDeath = artist.DateOfDeath,
        Biography = artist.Bio.Value,
        TribeId = artist.TribeId,
        CountryOfOrigin = artist.CountryOfOrigin,
        IsVerified = artist.IsVerified,
        IsActive = artist.IsActive,
        CreatedAt = artist.CreatedAt,
        UpdatedAt = artist.UpdatedAt
    };

    // ArtistName in ArtworkDto is denormalised; infrastructure populates it via join.
    // Service sets a safe default here so the contract is always satisfied.
    private static ArtworkDto MapArtworkToDto(Artwork artwork) => new()
    {
        Id = artwork.Id,
        Title = artwork.Title,
        ArtistId = artwork.ArtistId,
        ArtistName = string.Empty,
        YearCreated = artwork.YearCreated,
        Medium = artwork.ArtMedium.Value,
        WidthCm = artwork.Size.WidthCm,
        HeightCm = artwork.Size.HeightCm,
        ArtTypeId = artwork.ArtTypeId,
        Description = artwork.Description,
        AcquisitionDate = artwork.Acquisition.Date,
        AcquisitionPrice = artwork.Acquisition.Price,
        IsOnDisplay = artwork.IsOnDisplay,
        ImageUrl = artwork.ImageUrl,
        IsActive = artwork.IsActive,
        CreatedAt = artwork.CreatedAt,
        UpdatedAt = artwork.UpdatedAt
    };
}
