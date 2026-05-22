using AboriginalArtGallery.Application.Artworks;
using AboriginalArtGallery.Domain.Artworks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace AboriginalArtGallery.Tests.Services;

public class ArtworkServiceTests
{
    private readonly Mock<IArtworkRepository> _repoMock;
    private readonly ArtworkService _service;

    public ArtworkServiceTests()
    {
        _repoMock = new Mock<IArtworkRepository>();
        _service = new ArtworkService(_repoMock.Object, NullLogger<ArtworkService>.Instance);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((Artwork?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateAsync_CallsAddAsync_AndSaveChanges()
    {
        var dto = new CreateArtworkDto
        {
            Title = "Dreaming",
            ArtistId = Guid.NewGuid(),
            YearCreated = 1990,
            Medium = "Acrylic on canvas",
            WidthCm = 60.0m,
            HeightCm = 90.0m,
            ArtTypeId = null,
            Description = null,
            AcquisitionDate = null,
            AcquisitionPrice = null,
            ImageUrl = null
        };

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Artwork>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);

        await _service.CreateAsync(dto);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<Artwork>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SetDisplayStatusAsync_CallsUpdateAndSave()
    {
        var artwork = Artwork.Create(
            title: "Country",
            artistId: Guid.NewGuid(),
            yearCreated: 2005,
            medium: new Medium("Ochre on bark"),
            dimensions: new Dimensions(40.0m, 60.0m),
            artTypeId: null,
            description: null,
            acquisition: new AcquisitionInfo(null, null),
            imageUrl: null);

        _repoMock.Setup(r => r.GetByIdAsync(artwork.Id))
                 .ReturnsAsync(artwork);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Artwork>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);

        await _service.SetDisplayStatusAsync(artwork.Id, true);

        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Artwork>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((Artwork?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.DeleteAsync(Guid.NewGuid()));
    }
}
