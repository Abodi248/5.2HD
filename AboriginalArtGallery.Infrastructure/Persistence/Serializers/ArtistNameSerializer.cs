using AboriginalArtGallery.Domain.Artists;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AboriginalArtGallery.Infrastructure.Persistence.Serializers;

public class ArtistNameSerializer : SerializerBase<ArtistName>
{
    public override ArtistName Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var value = context.Reader.ReadString();
        return new ArtistName(value);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ArtistName value)
    {
        context.Writer.WriteString(value.Value);
    }
}
