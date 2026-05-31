using AboriginalArtGallery.Domain.ArtTypes;
using Microsoft.Extensions.Logging;

namespace AboriginalArtGallery.Application.ArtTypes;

public class ArtTypeService
{
    private readonly IArtTypeRepository _repository;
    private readonly ILogger<ArtTypeService> _logger;

    public ArtTypeService(IArtTypeRepository repository, ILogger<ArtTypeService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ArtTypeDto> GetByIdAsync(Guid id)
    {
        var artType = await _repository.GetByIdAsync(id);
        if (artType is null)
        {
            _logger.LogWarning("ArtType {Id} not found", id);
            throw new KeyNotFoundException($"ArtType {id} not found.");
        }
        _logger.LogInformation("Retrieved art type {Id}", id);
        return MapToDto(artType);
    }

    public async Task<(IEnumerable<ArtTypeDto> Items, int TotalCount)> GetAllAsync(
        string? search, string? category, int page, int pageSize)
    {
        var (items, total) = await _repository.GetAllAsync(search, category, page, pageSize);
        var dtoItems = items.Select(MapToDto).ToList();
        _logger.LogInformation("Retrieved {Count} art types (total: {Total})", dtoItems.Count, total);
        return (dtoItems, total);
    }

    public async Task<ArtTypeDto> CreateAsync(CreateArtTypeDto dto)
    {
        var artType = ArtType.Create(dto.Name, dto.Category, dto.Description);
        await _repository.AddAsync(artType);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Created art type {Id}", artType.Id);
        return MapToDto(artType);
    }

    public async Task<ArtTypeDto> UpdateAsync(Guid id, UpdateArtTypeDto dto)
    {
        var artType = await _repository.GetByIdAsync(id);
        if (artType is null)
        {
            _logger.LogWarning("ArtType {Id} not found for update", id);
            throw new KeyNotFoundException($"ArtType {id} not found.");
        }
        artType.Update(dto.Name, dto.Category, dto.Description);
        await _repository.UpdateAsync(artType);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Updated art type {Id}", id);
        return MapToDto(artType);
    }

    public async Task DeleteAsync(Guid id)
    {
        var artType = await _repository.GetByIdAsync(id);
        if (artType is null)
        {
            _logger.LogWarning("ArtType {Id} not found for deletion", id);
            throw new KeyNotFoundException($"ArtType {id} not found.");
        }
        artType.Deactivate();
        await _repository.UpdateAsync(artType);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Deactivated art type {Id}", id);
    }

    private static ArtTypeDto MapToDto(ArtType artType) => new()
    {
        Id = artType.Id,
        Name = artType.Name,
        Category = artType.Category,
        Description = artType.Description,
        IsActive = artType.IsActive,
        CreatedAt = artType.CreatedAt,
        UpdatedAt = artType.UpdatedAt
    };
}
