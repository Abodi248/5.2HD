using AboriginalArtGallery.Domain.Artworks;
using AboriginalArtGallery.Domain.Exceptions;

namespace AboriginalArtGallery.Tests.Domain;

public class AcquisitionInfoTests
{
    [Fact]
    public void Null_date_and_null_price_is_valid()
    {
        var info = new AcquisitionInfo(null, null);
        Assert.Null(info.Date);
        Assert.Null(info.Price);
    }

    [Fact]
    public void Negative_price_throws_DomainException()
    {
        Assert.Throws<DomainException>(() => new AcquisitionInfo(null, -1m));
    }

    [Fact]
    public void Zero_price_is_valid()
    {
        var info = new AcquisitionInfo(null, 0m);
        Assert.Equal(0m, info.Price);
    }

    [Fact]
    public void Valid_date_and_price_passes()
    {
        var date = new DateOnly(2020, 6, 15);
        var info = new AcquisitionInfo(date, 5000m);
        Assert.Equal(date, info.Date);
        Assert.Equal(5000m, info.Price);
    }
}
