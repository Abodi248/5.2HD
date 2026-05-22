using AboriginalArtGallery.Domain.Artworks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AboriginalArtGallery.Infrastructure.Persistence.Serializers;

public class DimensionsSerializer : SerializerBase<Dimensions>
{
    public override Dimensions Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        context.Reader.ReadStartDocument();
        decimal width = 0, height = 0;

        while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
        {
            var name = context.Reader.ReadName(Utf8NameDecoder.Instance);
            switch (name)
            {
                case "width_cm":
                    width = (decimal)context.Reader.ReadDecimal128();
                    break;
                case "height_cm":
                    height = (decimal)context.Reader.ReadDecimal128();
                    break;
                default:
                    context.Reader.SkipValue();
                    break;
            }
        }

        context.Reader.ReadEndDocument();
        return new Dimensions(width, height);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Dimensions value)
    {
        context.Writer.WriteStartDocument();
        context.Writer.WriteName("width_cm");
        context.Writer.WriteDecimal128(new Decimal128(value.WidthCm));
        context.Writer.WriteName("height_cm");
        context.Writer.WriteDecimal128(new Decimal128(value.HeightCm));
        context.Writer.WriteEndDocument();
    }
}
