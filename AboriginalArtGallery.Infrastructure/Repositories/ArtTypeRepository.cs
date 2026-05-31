using AboriginalArtGallery.Application.ArtTypes;
using AboriginalArtGallery.Domain.ArtTypes;
using AboriginalArtGallery.Infrastructure.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AboriginalArtGallery.Infrastructure.Repositories;

public class ArtTypeRepository : IArtTypeRepository
{
    private readonly MongoDbContext _context;

    public ArtTypeRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<ArtType?> GetByIdAsync(Guid id)
    {
        var filter = Builders<ArtType>.Filter.And(
            Builders<ArtType>.Filter.Eq(a => a.Id, id),
            Builders<ArtType>.Filter.Eq(a => a.IsActive, true));

        return await _context.ArtTypes.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<ArtType> Items, int TotalCount)> GetAllAsync(
        string? search, string? category, int page, int pageSize)
    {
        var filterList = new List<FilterDefinition<ArtType>>
        {
            Builders<ArtType>.Filter.Eq(a => a.IsActive, true)
        };

        if (!string.IsNullOrWhiteSpace(search))
            filterList.Add(Builders<ArtType>.Filter.Regex("name",
                new BsonRegularExpression(search.Trim(), "i")));

        if (!string.IsNullOrWhiteSpace(category))
            filterList.Add(Builders<ArtType>.Filter.Eq(a => a.Category, category));

        var combined = Builders<ArtType>.Filter.And(filterList);

        var normalPage = Math.Max(1, page);
        var normalPageSize = Math.Min(Math.Max(1, pageSize), 100);

        var totalCount = (int)await _context.ArtTypes.CountDocumentsAsync(combined);
        var items = await _context.ArtTypes
            .Find(combined)
            .Skip((normalPage - 1) * normalPageSize)
            .Limit(normalPageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(ArtType artType)
    {
        await _context.ArtTypes.InsertOneAsync(artType);
    }

    public async Task UpdateAsync(ArtType artType)
    {
        var filter = Builders<ArtType>.Filter.Eq(a => a.Id, artType.Id);
        await _context.ArtTypes.ReplaceOneAsync(filter, artType);
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}
