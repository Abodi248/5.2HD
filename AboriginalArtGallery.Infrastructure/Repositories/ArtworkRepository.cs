using AboriginalArtGallery.Application.Artworks;
using AboriginalArtGallery.Domain.Artworks;
using AboriginalArtGallery.Infrastructure.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AboriginalArtGallery.Infrastructure.Repositories;

public class ArtworkRepository : IArtworkRepository
{
    private readonly MongoDbContext _context;

    public ArtworkRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Artwork?> GetByIdAsync(Guid id)
    {
        var filter = Builders<Artwork>.Filter.And(
            Builders<Artwork>.Filter.Eq(a => a.Id, id),
            Builders<Artwork>.Filter.Eq(a => a.IsActive, true));

        return await _context.Artworks.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<Artwork> Items, int TotalCount)> GetAllAsync(ArtworkFilterParams filters)
    {
        var filterList = new List<FilterDefinition<Artwork>>
        {
            Builders<Artwork>.Filter.Eq(a => a.IsActive, true)
        };

        if (filters.ArtistId.HasValue)
            filterList.Add(Builders<Artwork>.Filter.Eq(a => a.ArtistId, filters.ArtistId.Value));

        if (filters.OnDisplay.HasValue)
            filterList.Add(Builders<Artwork>.Filter.Eq(a => a.IsOnDisplay, filters.OnDisplay.Value));

        if (filters.ArtTypeId.HasValue)
            filterList.Add(Builders<Artwork>.Filter.Eq(a => a.ArtTypeId, filters.ArtTypeId.Value));

        if (!string.IsNullOrWhiteSpace(filters.Medium))
            filterList.Add(Builders<Artwork>.Filter.Regex("medium",
                new BsonRegularExpression(filters.Medium.Trim(), "i")));

        if (filters.YearFrom.HasValue)
            filterList.Add(Builders<Artwork>.Filter.Gte(a => a.YearCreated, filters.YearFrom.Value));

        if (filters.YearTo.HasValue)
            filterList.Add(Builders<Artwork>.Filter.Lte(a => a.YearCreated, filters.YearTo.Value));

        var combined = Builders<Artwork>.Filter.And(filterList);

        var page = Math.Max(1, filters.Page);
        var pageSize = Math.Min(Math.Max(1, filters.PageSize), 100);

        var totalCount = (int)await _context.Artworks.CountDocumentsAsync(combined);
        var items = await _context.Artworks
            .Find(combined)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Artwork artwork)
    {
        await _context.Artworks.InsertOneAsync(artwork);
    }

    public async Task UpdateAsync(Artwork artwork)
    {
        var filter = Builders<Artwork>.Filter.Eq(a => a.Id, artwork.Id);
        await _context.Artworks.ReplaceOneAsync(filter, artwork);
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}
