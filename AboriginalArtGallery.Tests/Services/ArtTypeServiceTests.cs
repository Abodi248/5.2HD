using AboriginalArtGallery.Application.ArtTypes;
using AboriginalArtGallery.Domain.ArtTypes;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace AboriginalArtGallery.Tests.Services;

public class ArtTypeServiceTests
{
    private readonly Mock<IArtTypeRepository> _repoMock;
    private readonly ArtTypeService _service;

    public ArtTypeServiceTests()
    {
        _repoMock = new Mock<IArtTypeRepository>();
        _service = new ArtTypeService(_repoMock.Object, NullLogger<ArtTypeService>.Instance);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsDto_WhenFound()
    {
        var artType = ArtType.Create("Dot Painting", "Painting", null);

        _repoMock.Setup(r => r.GetByIdAsync(artType.Id))
                 .ReturnsAsync(artType);

        var result = await _service.GetByIdAsync(artType.Id);

        Assert.Equal(artType.Id, result.Id);
        Assert.Equal(artType.Name, result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((ArtType?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateAsync_CallsAddAsync_AndSaveChanges()
    {
        var dto = new CreateArtTypeDto
        {
            Name = "Bark Painting",
            Category = "Painting",
            Description = null
        };

        _repoMock.Setup(r => r.AddAsync(It.IsAny<ArtType>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);

        await _service.CreateAsync(dto);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<ArtType>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((ArtType?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.DeleteAsync(Guid.NewGuid()));
    }
}
