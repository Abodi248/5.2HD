using AboriginalArtGallery.Application.Artists;
using AboriginalArtGallery.Domain.Artists;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace AboriginalArtGallery.Tests.Services;

public class ArtistServiceTests
{
    private readonly Mock<IArtistRepository> _repoMock;
    private readonly ArtistService _service;

    public ArtistServiceTests()
    {
        _repoMock = new Mock<IArtistRepository>();
        _service = new ArtistService(_repoMock.Object, NullLogger<ArtistService>.Instance);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsDto_WhenArtistExists()
    {
        var artist = Artist.Create(
            new ArtistName("Albert Namatjira"),
            knownAs: null,
            dob: null,
            dod: null,
            new Biography(null),
            tribeId: null,
            countryOfOrigin: "Australia");

        _repoMock.Setup(r => r.GetByIdAsync(artist.Id))
                 .ReturnsAsync(artist);

        var result = await _service.GetByIdAsync(artist.Id);

        Assert.Equal(artist.Id, result.Id);
        Assert.Equal(artist.Name.Value, result.FullName);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((Artist?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateAsync_CallsAddAsync_AndSaveChanges()
    {
        var dto = new CreateArtistDto
        {
            FullName = "Emily Kame Kngwarreye",
            KnownAs = "Emily",
            DateOfBirth = null,
            DateOfDeath = null,
            Biography = null,
            TribeId = null,
            CountryOfOrigin = "Australia"
        };

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Artist>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);

        await _service.CreateAsync(dto);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<Artist>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetSummaryAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetSummaryAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((ArtistSummaryDto?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetSummaryAsync(Guid.NewGuid()));
    }
}
