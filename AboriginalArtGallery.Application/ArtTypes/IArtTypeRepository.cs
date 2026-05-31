using AboriginalArtGallery.Domain.ArtTypes;

namespace AboriginalArtGallery.Application.ArtTypes;

public interface IArtTypeRepository
{
    Task<ArtType?> GetByIdAsync(Guid id);
    Task<(IEnumerable<ArtType> Items, int TotalCount)> GetAllAsync(
        string? search, string? category, int page, int pageSize);
    Task AddAsync(ArtType artType);
    Task UpdateAsync(ArtType artType);
    Task SaveChangesAsync();
}
