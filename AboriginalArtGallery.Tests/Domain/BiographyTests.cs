using AboriginalArtGallery.Domain.Artists;
using AboriginalArtGallery.Domain.Exceptions;

namespace AboriginalArtGallery.Tests.Domain;

public class BiographyTests
{
    [Fact]
    public void NullBiography_IsValid()
    {
        var bio = new Biography(null);
        Assert.Null(bio.Value);
    }

    [Fact]
    public void EmptyString_IsValid()
    {
        var bio = new Biography(string.Empty);
        Assert.Equal(string.Empty, bio.Value);
    }

    [Fact]
    public void BiographyOver5000Chars_ThrowsDomainException()
    {
        var longText = new string('A', 5001);
        Assert.Throws<DomainException>(() => new Biography(longText));
    }

    [Fact]
    public void BiographyExactly5000Chars_IsValid()
    {
        var text = new string('A', 5000);
        var bio = new Biography(text);
        Assert.Equal(5000, bio.Value!.Length);
    }
}
