using AboriginalArtGallery.Application.Artists;
using AboriginalArtGallery.Application.ArtTypes;
using AboriginalArtGallery.Application.Artworks;
using AboriginalArtGallery.Application.Tribes;
using AboriginalArtGallery.Infrastructure.Persistence;
using AboriginalArtGallery.Infrastructure.Persistence.Serializers;
using AboriginalArtGallery.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace AboriginalArtGallery.Infrastructure;

public static class MongoDbServiceExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterSerializers();

        var connectionString = configuration["MongoDbSettings:ConnectionString"]
            ?? throw new InvalidOperationException("MongoDbSettings:ConnectionString is missing.");
        var databaseName = configuration["MongoDbSettings:DatabaseName"]
            ?? throw new InvalidOperationException("MongoDbSettings:DatabaseName is missing.");

        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseName);

        MongoDbInitializer.CreateIndexes(mongoDatabase);

        services.AddSingleton<IMongoClient>(mongoClient);
        services.AddSingleton<IMongoDatabase>(mongoDatabase);
        services.AddSingleton<MongoDbContext>();

        services.AddSingleton<IArtistRepository, ArtistRepository>();
        services.AddSingleton<IArtworkRepository, ArtworkRepository>();
        services.AddSingleton<ITribeRepository, TribeRepository>();
        services.AddSingleton<IArtTypeRepository, ArtTypeRepository>();

        return services;
    }

    private static void RegisterSerializers()
    {
        BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.TryRegisterSerializer(new ArtistNameSerializer());
        BsonSerializer.TryRegisterSerializer(new BiographySerializer());
        BsonSerializer.TryRegisterSerializer(new MediumSerializer());
        BsonSerializer.TryRegisterSerializer(new DimensionsSerializer());
        BsonSerializer.TryRegisterSerializer(new AcquisitionInfoSerializer());
    }
}
