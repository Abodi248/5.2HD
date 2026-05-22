using AboriginalArtGallery.Domain.Artworks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AboriginalArtGallery.Infrastructure.Persistence.Serializers;

public class MediumSerializer : SerializerBase<Medium>
{
    public override Medium Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var value = context.Reader.ReadString();
        return new Medium(value);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Medium value)
    {
        context.Writer.WriteString(value.Value);
    }
}
