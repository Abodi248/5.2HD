using AboriginalArtGallery.Domain.Artists;
using AboriginalArtGallery.Domain.Exceptions;

namespace AboriginalArtGallery.Tests.Domain;

public class ArtistNameTests
{
    [Fact]
    public void Valid_name_passes_without_throwing()
    {
        var name = new ArtistName("Albert Namatjira");
        Assert.Equal("Albert Namatjira", name.Value);
    }

    [Fact]
    public void Empty_string_throws_DomainException()
    {
        Assert.Throws<DomainException>(() => new ArtistName(""));
    }

    [Fact]
    public void Whitespace_only_throws_DomainException()
    {
        Assert.Throws<DomainException>(() => new ArtistName("   "));
    }

    [Fact]
    public void Name_over_200_characters_throws_DomainException()
    {
        var longName = new string('A', 201);
        Assert.Throws<DomainException>(() => new ArtistName(longName));
    }

    [Fact]
    public void Name_is_trimmed()
    {
        var name = new ArtistName("  Albert Namatjira  ");
        Assert.Equal("Albert Namatjira", name.Value);
    }
}
