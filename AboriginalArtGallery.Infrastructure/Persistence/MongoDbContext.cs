using AboriginalArtGallery.Domain.Artists;
using AboriginalArtGallery.Domain.ArtTypes;
using AboriginalArtGallery.Domain.Artworks;
using AboriginalArtGallery.Domain.Tribes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AboriginalArtGallery.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _db;

    public MongoDbContext(IMongoDatabase db)
    {
        _db = db;
    }

    public IMongoCollection<Artist> Artists => _db.GetCollection<Artist>("artists");
    public IMongoCollection<Artwork> Artworks => _db.GetCollection<Artwork>("artworks");
    public IMongoCollection<Tribe> Tribes => _db.GetCollection<Tribe>("tribes");
    public IMongoCollection<ArtType> ArtTypes => _db.GetCollection<ArtType>("art_types");
}

public static class MongoDbInitializer
{
    public static void CreateIndexes(IMongoDatabase db)
    {
        CreateArtistIndexes(db);
        CreateArtworkIndexes(db);
        CreateTribeIndexes(db);
        CreateArtTypeIndexes(db);
    }

    private static void CreateArtistIndexes(IMongoDatabase db)
    {
        var col = db.GetCollection<BsonDocument>("artists");

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Text("full_name"),
            new CreateIndexOptions { Name = "idx_artists_full_name_text" }));

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Ascending("tribe_id"),
            new CreateIndexOptions { Name = "idx_artists_tribe_id", Sparse = true }));

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Ascending("is_verified"),
            new CreateIndexOptions { Name = "idx_artists_is_verified" }));

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Ascending("is_active"),
            new CreateIndexOptions { Name = "idx_artists_is_active" }));
    }

    private static void CreateArtworkIndexes(IMongoDatabase db)
    {
        var col = db.GetCollection<BsonDocument>("artworks");

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Ascending("artist_id"),
            new CreateIndexOptions { Name = "idx_artworks_artist_id" }));

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Ascending("is_on_display"),
            new CreateIndexOptions<BsonDocument>
            {
                Name = "idx_artworks_on_display_partial",
                PartialFilterExpression = Builders<BsonDocument>.Filter.Eq("is_on_display", true)
            }));

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Ascending("year_created"),
            new CreateIndexOptions<BsonDocument>
            {
                Name = "idx_artworks_year_created_partial",
                PartialFilterExpression = Builders<BsonDocument>.Filter.Exists("year_created")
            }));

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Ascending("is_active"),
            new CreateIndexOptions { Name = "idx_artworks_is_active" }));

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Combine(
                Builders<BsonDocument>.IndexKeys.Text("title"),
                Builders<BsonDocument>.IndexKeys.Text("medium")),
            new CreateIndexOptions { Name = "idx_artworks_title_medium_text" }));
    }

    private static void CreateTribeIndexes(IMongoDatabase db)
    {
        var col = db.GetCollection<BsonDocument>("tribes");

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Ascending("is_active"),
            new CreateIndexOptions { Name = "idx_tribes_is_active" }));

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Ascending("region"),
            new CreateIndexOptions { Name = "idx_tribes_region" }));

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Text("name"),
            new CreateIndexOptions { Name = "idx_tribes_name_text" }));
    }

    private static void CreateArtTypeIndexes(IMongoDatabase db)
    {
        var col = db.GetCollection<BsonDocument>("art_types");

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Ascending("is_active"),
            new CreateIndexOptions { Name = "idx_art_types_is_active" }));

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Ascending("category"),
            new CreateIndexOptions { Name = "idx_art_types_category" }));

        col.Indexes.CreateOne(new CreateIndexModel<BsonDocument>(
            Builders<BsonDocument>.IndexKeys.Text("name"),
            new CreateIndexOptions { Name = "idx_art_types_name_text" }));
    }
}
