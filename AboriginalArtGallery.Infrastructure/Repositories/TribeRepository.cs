using AboriginalArtGallery.Application.Tribes;
using AboriginalArtGallery.Domain.Tribes;
using AboriginalArtGallery.Infrastructure.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AboriginalArtGallery.Infrastructure.Repositories;

public class TribeRepository : ITribeRepository
{
    private readonly MongoDbContext _context;

    public TribeRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Tribe?> GetByIdAsync(Guid id)
    {
        var filter = Builders<Tribe>.Filter.And(
            Builders<Tribe>.Filter.Eq(t => t.Id, id),
            Builders<Tribe>.Filter.Eq(t => t.IsActive, true));

        return await _context.Tribes.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<Tribe> Items, int TotalCount)> GetAllAsync(
        string? search, int page, int pageSize)
    {
        var filterList = new List<FilterDefinition<Tribe>>
        {
            Builders<Tribe>.Filter.Eq(t => t.IsActive, true)
        };

        if (!string.IsNullOrWhiteSpace(search))
            filterList.Add(Builders<Tribe>.Filter.Regex("name",
                new BsonRegularExpression(search.Trim(), "i")));

        var combined = Builders<Tribe>.Filter.And(filterList);

        var normalPage = Math.Max(1, page);
        var normalPageSize = Math.Min(Math.Max(1, pageSize), 100);

        var totalCount = (int)await _context.Tribes.CountDocumentsAsync(combined);
        var items = await _context.Tribes
            .Find(combined)
            .Skip((normalPage - 1) * normalPageSize)
            .Limit(normalPageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Tribe tribe)
    {
        await _context.Tribes.InsertOneAsync(tribe);
    }

    public async Task UpdateAsync(Tribe tribe)
    {
        var filter = Builders<Tribe>.Filter.Eq(t => t.Id, tribe.Id);
        await _context.Tribes.ReplaceOneAsync(filter, tribe);
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}
