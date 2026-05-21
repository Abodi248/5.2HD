using AboriginalArtGallery.Domain.Exceptions;

namespace AboriginalArtGallery.Domain.Artworks;

public record AcquisitionInfo
{
    public DateOnly? Date { get; }
    public decimal? Price { get; }

    public AcquisitionInfo(DateOnly? date, decimal? price)
    {
        if (price is not null && price < 0)
            throw new DomainException("Acquisition price cannot be negative.");

        Date = date;
        Price = price;
    }
}
