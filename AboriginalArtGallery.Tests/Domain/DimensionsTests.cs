using AboriginalArtGallery.Domain.Artworks;
using AboriginalArtGallery.Domain.Exceptions;

namespace AboriginalArtGallery.Tests.Domain;

public class DimensionsTests
{
    [Fact]
    public void Valid_positive_decimals_pass()
    {
        var d = new Dimensions(30.5m, 45.0m);
        Assert.Equal(30.5m, d.WidthCm);
        Assert.Equal(45.0m, d.HeightCm);
    }

    [Fact]
    public void Width_zero_throws_DomainException()
    {
        Assert.Throws<DomainException>(() => new Dimensions(0m, 10m));
    }

    [Fact]
    public void Negative_height_throws_DomainException()
    {
        Assert.Throws<DomainException>(() => new Dimensions(10m, -1m));
    }

    [Fact]
    public void Both_zero_throws_DomainException()
    {
        Assert.Throws<DomainException>(() => new Dimensions(0m, 0m));
    }
}
