using AboriginalArtGallery.Domain.Artworks;
using Microsoft.Extensions.Logging;

namespace AboriginalArtGallery.Application.Artworks;

public class ArtworkService
{
    private readonly IArtworkRepository _repository;
    private readonly ILogger<ArtworkService> _logger;

    public ArtworkService(IArtworkRepository repository, ILogger<ArtworkService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ArtworkDto> GetByIdAsync(Guid id)
    {
        var artwork = await _repository.GetByIdAsync(id);
        if (artwork is null)
        {
            _logger.LogWarning("Artwork {Id} not found", id);
            throw new KeyNotFoundException($"Artwork {id} not found.");
        }
        _logger.LogInformation("Retrieved artwork {Id}", id);
        return MapToDto(artwork);
    }

    public async Task<(IEnumerable<ArtworkDto> Items, int TotalCount)> GetAllAsync(ArtworkFilterParams filters)
    {
        var (items, total) = await _repository.GetAllAsync(filters);
        var dtoItems = items.Select(MapToDto).ToList();
        _logger.LogInformation("Retrieved {Count} artworks (total: {Total})", dtoItems.Count, total);
        return (dtoItems, total);
    }

    public async Task<ArtworkDto> CreateAsync(CreateArtworkDto dto)
    {
        var medium = new Medium(dto.Medium);
        var dimensions = new Dimensions(dto.WidthCm, dto.HeightCm);
        var acquisition = new AcquisitionInfo(dto.AcquisitionDate, dto.AcquisitionPrice);
        var artwork = Artwork.Create(dto.Title, dto.ArtistId, dto.YearCreated, medium, dimensions,
            dto.ArtTypeId, dto.Description, acquisition, dto.ImageUrl);
        await _repository.AddAsync(artwork);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Created artwork {Id}", artwork.Id);
        return MapToDto(artwork);
    }

    public async Task<ArtworkDto> UpdateAsync(Guid id, UpdateArtworkDto dto)
    {
        var artwork = await _repository.GetByIdAsync(id);
        if (artwork is null)
        {
            _logger.LogWarning("Artwork {Id} not found for update", id);
            throw new KeyNotFoundException($"Artwork {id} not found.");
        }
        var medium = new Medium(dto.Medium);
        var dimensions = new Dimensions(dto.WidthCm, dto.HeightCm);
        var acquisition = new AcquisitionInfo(dto.AcquisitionDate, dto.AcquisitionPrice);
        artwork.Update(dto.Title, dto.YearCreated, medium, dimensions,
            dto.ArtTypeId, dto.Description, acquisition, dto.ImageUrl);
        await _repository.UpdateAsync(artwork);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Updated artwork {Id}", id);
        return MapToDto(artwork);
    }

    public async Task<ArtworkDto> SetDisplayStatusAsync(Guid id, bool isOnDisplay)
    {
        var artwork = await _repository.GetByIdAsync(id);
        if (artwork is null)
        {
            _logger.LogWarning("Artwork {Id} not found for display status update", id);
            throw new KeyNotFoundException($"Artwork {id} not found.");
        }
        artwork.SetDisplayStatus(isOnDisplay);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Set display status of artwork {Id} to {Status}", id, isOnDisplay);
        return MapToDto(artwork);
    }

    public async Task DeleteAsync(Guid id)
    {
        var artwork = await _repository.GetByIdAsync(id);
        if (artwork is null)
        {
            _logger.LogWarning("Artwork {Id} not found for deletion", id);
            throw new KeyNotFoundException($"Artwork {id} not found.");
        }
        artwork.Deactivate();
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Deactivated artwork {Id}", id);
    }

    // ArtistName is denormalised; infrastructure populates it via join when querying.
    private static ArtworkDto MapToDto(Artwork artwork) => new()
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
