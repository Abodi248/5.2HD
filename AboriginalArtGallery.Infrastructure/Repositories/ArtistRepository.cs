using AboriginalArtGallery.Application.Artists;
using AboriginalArtGallery.Application.Artworks;
using AboriginalArtGallery.Domain.Artists;
using AboriginalArtGallery.Domain.Artworks;
using AboriginalArtGallery.Infrastructure.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AboriginalArtGallery.Infrastructure.Repositories;

public class ArtistRepository : IArtistRepository
{
    private readonly MongoDbContext _context;

    public ArtistRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Artist?> GetByIdAsync(Guid id)
    {
        var filter = Builders<Artist>.Filter.And(
            Builders<Artist>.Filter.Eq(a => a.Id, id),
            Builders<Artist>.Filter.Eq(a => a.IsActive, true));

        return await _context.Artists.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<Artist> Items, int TotalCount)> GetAllAsync(ArtistFilterParams filters)
    {
        var filterList = new List<FilterDefinition<Artist>>
        {
            Builders<Artist>.Filter.Eq(a => a.IsActive, true)
        };

        if (!string.IsNullOrWhiteSpace(filters.Search))
            filterList.Add(Builders<Artist>.Filter.Regex("full_name",
                new BsonRegularExpression(filters.Search.Trim(), "i")));

        if (filters.TribeId.HasValue)
            filterList.Add(Builders<Artist>.Filter.Eq(a => a.TribeId, filters.TribeId.Value));

        if (filters.IsVerified.HasValue)
            filterList.Add(Builders<Artist>.Filter.Eq(a => a.IsVerified, filters.IsVerified.Value));

        var combined = Builders<Artist>.Filter.And(filterList);

        var page = Math.Max(1, filters.Page);
        var pageSize = Math.Min(Math.Max(1, filters.PageSize), 100);

        var totalCount = (int)await _context.Artists.CountDocumentsAsync(combined);
        var items = await _context.Artists
            .Find(combined)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Artwork>> GetArtworksByArtistIdAsync(Guid artistId)
    {
        var filter = Builders<Artwork>.Filter.And(
            Builders<Artwork>.Filter.Eq(a => a.ArtistId, artistId),
            Builders<Artwork>.Filter.Eq(a => a.IsActive, true));

        return await _context.Artworks.Find(filter).ToListAsync();
    }

    public async Task<ArtistSummaryDto?> GetSummaryAsync(Guid artistId)
    {
        var artist = await GetByIdAsync(artistId);
        if (artist is null)
            return null;

        var artworkFilter = Builders<Artwork>.Filter.And(
            Builders<Artwork>.Filter.Eq(a => a.ArtistId, artistId),
            Builders<Artwork>.Filter.Eq(a => a.IsActive, true));

        var artworks = await _context.Artworks.Find(artworkFilter).ToListAsync();

        var years = artworks.Where(a => a.YearCreated.HasValue).Select(a => a.YearCreated!.Value).ToList();
        var mediaUsed = artworks.Select(a => a.ArtMedium.Value).Distinct().OrderBy(m => m).ToList();

        return new ArtistSummaryDto
        {
            ArtistId = artistId,
            FullName = artist.Name.Value,
            ArtworkCount = artworks.Count,
            EarliestYear = years.Count > 0 ? years.Min() : null,
            LatestYear = years.Count > 0 ? years.Max() : null,
            MediaUsed = mediaUsed
        };
    }

    public async Task AddAsync(Artist artist)
    {
        await _context.Artists.InsertOneAsync(artist);
    }

    public async Task UpdateAsync(Artist artist)
    {
        var filter = Builders<Artist>.Filter.Eq(a => a.Id, artist.Id);
        await _context.Artists.ReplaceOneAsync(filter, artist);
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}
