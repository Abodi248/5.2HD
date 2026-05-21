using AboriginalArtGallery.Domain.Exceptions;

namespace AboriginalArtGallery.Domain.Artworks;

public record Dimensions
{
    public decimal WidthCm { get; }
    public decimal HeightCm { get; }

    public Dimensions(decimal widthCm, decimal heightCm)
    {
        if (widthCm <= 0)
            throw new DomainException("Width must be greater than zero.");

        if (heightCm <= 0)
            throw new DomainException("Height must be greater than zero.");

        WidthCm = widthCm;
        HeightCm = heightCm;
    }
}
