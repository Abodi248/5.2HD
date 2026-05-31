using AboriginalArtGallery.Domain.Tribes;

namespace AboriginalArtGallery.Application.Tribes;

public interface ITribeRepository
{
    Task<Tribe?> GetByIdAsync(Guid id);
    Task<(IEnumerable<Tribe> Items, int TotalCount)> GetAllAsync(string? search, int page, int pageSize);
    Task AddAsync(Tribe tribe);
    Task UpdateAsync(Tribe tribe);
    Task SaveChangesAsync();
}
