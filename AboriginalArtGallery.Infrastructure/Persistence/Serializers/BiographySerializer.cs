using AboriginalArtGallery.Domain.Artists;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AboriginalArtGallery.Infrastructure.Persistence.Serializers;

public class BiographySerializer : SerializerBase<Biography>
{
    public override Biography Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Null)
        {
            context.Reader.ReadNull();
            return new Biography(null);
        }
        var value = context.Reader.ReadString();
        return new Biography(value);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Biography value)
    {
        if (value?.Value is null)
            context.Writer.WriteNull();
        else
            context.Writer.WriteString(value.Value);
    }
}
