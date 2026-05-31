using AboriginalArtGallery.Domain.Tribes;
using Microsoft.Extensions.Logging;

namespace AboriginalArtGallery.Application.Tribes;

public class TribeService
{
    private readonly ITribeRepository _repository;
    private readonly ILogger<TribeService> _logger;

    public TribeService(ITribeRepository repository, ILogger<TribeService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<TribeDto> GetByIdAsync(Guid id)
    {
        var tribe = await _repository.GetByIdAsync(id);
        if (tribe is null)
        {
            _logger.LogWarning("Tribe {Id} not found", id);
            throw new KeyNotFoundException($"Tribe {id} not found.");
        }
        _logger.LogInformation("Retrieved tribe {Id}", id);
        return MapToDto(tribe);
    }

    public async Task<(IEnumerable<TribeDto> Items, int TotalCount)> GetAllAsync(
        string? search, int page, int pageSize)
    {
        var (items, total) = await _repository.GetAllAsync(search, page, pageSize);
        var dtoItems = items.Select(MapToDto).ToList();
        _logger.LogInformation("Retrieved {Count} tribes (total: {Total})", dtoItems.Count, total);
        return (dtoItems, total);
    }

    public async Task<TribeDto> CreateAsync(CreateTribeDto dto)
    {
        var tribe = Tribe.Create(dto.Name, dto.Region, dto.LanguageGroup, dto.Description);
        await _repository.AddAsync(tribe);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Created tribe {Id}", tribe.Id);
        return MapToDto(tribe);
    }

    public async Task<TribeDto> UpdateAsync(Guid id, UpdateTribeDto dto)
    {
        var tribe = await _repository.GetByIdAsync(id);
        if (tribe is null)
        {
            _logger.LogWarning("Tribe {Id} not found for update", id);
            throw new KeyNotFoundException($"Tribe {id} not found.");
        }
        tribe.Update(dto.Name, dto.Region, dto.LanguageGroup, dto.Description);
        await _repository.UpdateAsync(tribe);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Updated tribe {Id}", id);
        return MapToDto(tribe);
    }

    public async Task DeleteAsync(Guid id)
    {
        var tribe = await _repository.GetByIdAsync(id);
        if (tribe is null)
        {
            _logger.LogWarning("Tribe {Id} not found for deletion", id);
            throw new KeyNotFoundException($"Tribe {id} not found.");
        }
        tribe.Deactivate();
        await _repository.UpdateAsync(tribe);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Deactivated tribe {Id}", id);
    }

    private static TribeDto MapToDto(Tribe tribe) => new()
    {
        Id = tribe.Id,
        Name = tribe.Name,
        Region = tribe.Region,
        LanguageGroup = tribe.LanguageGroup,
        Description = tribe.Description,
        IsActive = tribe.IsActive,
        CreatedAt = tribe.CreatedAt,
        UpdatedAt = tribe.UpdatedAt
    };
}
