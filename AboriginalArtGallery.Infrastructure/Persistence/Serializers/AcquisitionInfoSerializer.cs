using AboriginalArtGallery.Domain.Artworks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AboriginalArtGallery.Infrastructure.Persistence.Serializers;

public class AcquisitionInfoSerializer : SerializerBase<AcquisitionInfo>
{
    public override AcquisitionInfo Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        context.Reader.ReadStartDocument();
        DateOnly? date = null;
        decimal? price = null;

        while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
        {
            var name = context.Reader.ReadName(Utf8NameDecoder.Instance);
            switch (name)
            {
                case "date":
                    if (context.Reader.CurrentBsonType == BsonType.Null)
                        context.Reader.ReadNull();
                    else
                        date = DateOnly.ParseExact(context.Reader.ReadString(), "yyyy-MM-dd");
                    break;
                case "price":
                    if (context.Reader.CurrentBsonType == BsonType.Null)
                        context.Reader.ReadNull();
                    else
                        price = (decimal)context.Reader.ReadDecimal128();
                    break;
                default:
                    context.Reader.SkipValue();
                    break;
            }
        }

        context.Reader.ReadEndDocument();
        return new AcquisitionInfo(date, price);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, AcquisitionInfo value)
    {
        context.Writer.WriteStartDocument();

        context.Writer.WriteName("date");
        if (value.Date is null)
            context.Writer.WriteNull();
        else
            context.Writer.WriteString(value.Date.Value.ToString("yyyy-MM-dd"));

        context.Writer.WriteName("price");
        if (value.Price is null)
            context.Writer.WriteNull();
        else
            context.Writer.WriteDecimal128(new Decimal128(value.Price.Value));

        context.Writer.WriteEndDocument();
    }
}
