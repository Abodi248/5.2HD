using AboriginalArtGallery.Domain.Artists;
using AboriginalArtGallery.Domain.Artworks;

namespace AboriginalArtGallery.Application.Artists;

public interface IArtistRepository
{
    Task<Artist?> GetByIdAsync(Guid id);
    Task<(IEnumerable<Artist> Items, int TotalCount)> GetAllAsync(ArtistFilterParams filters);
    Task<IEnumerable<Artwork>> GetArtworksByArtistIdAsync(Guid artistId);
    Task<ArtistSummaryDto?> GetSummaryAsync(Guid artistId);
    Task AddAsync(Artist artist);
    Task UpdateAsync(Artist artist);
    Task SaveChangesAsync();
}
