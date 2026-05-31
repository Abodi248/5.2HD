using AboriginalArtGallery.Application.Tribes;
using AboriginalArtGallery.Domain.Tribes;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace AboriginalArtGallery.Tests.Services;

public class TribeServiceTests
{
    private readonly Mock<ITribeRepository> _repoMock;
    private readonly TribeService _service;

    public TribeServiceTests()
    {
        _repoMock = new Mock<ITribeRepository>();
        _service = new TribeService(_repoMock.Object, NullLogger<TribeService>.Instance);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsDto_WhenFound()
    {
        var tribe = Tribe.Create("Arrernte", "Central Australia", null, null);

        _repoMock.Setup(r => r.GetByIdAsync(tribe.Id))
                 .ReturnsAsync(tribe);

        var result = await _service.GetByIdAsync(tribe.Id);

        Assert.Equal(tribe.Id, result.Id);
        Assert.Equal(tribe.Name, result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((Tribe?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateAsync_CallsAddAsync_AndSaveChanges()
    {
        var dto = new CreateTribeDto
        {
            Name = "Yolngu",
            Region = "Arnhem Land",
            LanguageGroup = null,
            Description = null
        };

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Tribe>()))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync())
                 .Returns(Task.CompletedTask);

        await _service.CreateAsync(dto);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<Tribe>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsKeyNotFoundException_WhenNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                 .ReturnsAsync((Tribe?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _service.DeleteAsync(Guid.NewGuid()));
    }
}
