using AboriginalArtGallery.Domain.Artworks;

namespace AboriginalArtGallery.Application.Artworks;

public interface IArtworkRepository
{
    Task<Artwork?> GetByIdAsync(Guid id);
    Task<(IEnumerable<Artwork> Items, int TotalCount)> GetAllAsync(ArtworkFilterParams filters);
    Task AddAsync(Artwork artwork);
    Task UpdateAsync(Artwork artwork);
    Task SaveChangesAsync();
}
