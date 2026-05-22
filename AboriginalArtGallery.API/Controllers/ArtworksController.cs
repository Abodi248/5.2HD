using AboriginalArtGallery.API.Models;
using AboriginalArtGallery.Application.Artworks;
using Microsoft.AspNetCore.Mvc;

namespace AboriginalArtGallery.API.Controllers;

/// <summary>Manages artworks held by the Aboriginal Art Gallery.</summary>
[ApiController]
[Route("api/artworks")]
public class ArtworksController : ControllerBase
{
    private readonly ArtworkService _service;

    public ArtworksController(ArtworkService service)
    {
        _service = service;
    }

    /// <summary>Returns a paginated list of artworks.</summary>
    /// <param name="artistId">Filter by the creating artist's ID.</param>
    /// <param name="onDisplay">Filter by whether the artwork is currently on display.</param>
    /// <param name="medium">Filter by medium (case-insensitive partial match).</param>
    /// <param name="yearFrom">Include only artworks created from this year onwards.</param>
    /// <param name="yearTo">Include only artworks created up to and including this year.</param>
    /// <param name="artTypeId">Filter by art type ID.</param>
    /// <param name="page">Page number, 1-based (default: 1).</param>
    /// <param name="pageSize">Items per page, max 100 (default: 20).</param>
    /// <returns>A paginated envelope containing matching artworks.</returns>
    /// <response code="200">Returns the paginated artwork list.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ArtworkDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ArtworkDto>>> GetAll(
        [FromQuery] Guid? artistId,
        [FromQuery] bool? onDisplay,
        [FromQuery] string? medium,
        [FromQuery] int? yearFrom,
        [FromQuery] int? yearTo,
        [FromQuery] Guid? artTypeId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var filters = new ArtworkFilterParams
        {
            ArtistId = artistId,
            OnDisplay = onDisplay,
            Medium = medium,
            YearFrom = yearFrom,
            YearTo = yearTo,
            ArtTypeId = artTypeId,
            Page = page,
            PageSize = pageSize
        };

        var (items, totalCount) = await _service.GetAllAsync(filters);

        return Ok(new PagedResult<ArtworkDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    /// <summary>Returns a single artwork by ID.</summary>
    /// <param name="id">The artwork's unique identifier.</param>
    /// <returns>The artwork with the specified ID.</returns>
    /// <response code="200">Returns the artwork.</response>
    /// <response code="404">Artwork not found or has been deactivated.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ArtworkDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArtworkDto>> GetById(Guid id)
    {
        var artwork = await _service.GetByIdAsync(id);
        return Ok(artwork);
    }

    /// <summary>Creates a new artwork.</summary>
    /// <param name="dto">The artwork data to create.</param>
    /// <returns>The newly created artwork, with a Location header pointing to the resource.</returns>
    /// <response code="201">Artwork created successfully.</response>
    /// <response code="400">Validation failed or a domain rule was violated.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ArtworkDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ArtworkDto>> Create([FromBody] CreateArtworkDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Updates an existing artwork.</summary>
    /// <param name="id">The artwork's unique identifier.</param>
    /// <param name="dto">The replacement artwork data.</param>
    /// <returns>The updated artwork.</returns>
    /// <response code="200">Artwork updated successfully.</response>
    /// <response code="404">Artwork not found.</response>
    /// <response code="400">Validation failed or a domain rule was violated.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ArtworkDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ArtworkDto>> Update(Guid id, [FromBody] UpdateArtworkDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return Ok(updated);
    }

    /// <summary>Soft deletes an artwork by setting is_active to false.</summary>
    /// <param name="id">The artwork's unique identifier.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Artwork deactivated successfully.</response>
    /// <response code="404">Artwork not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>Updates the display status of an artwork.</summary>
    /// <param name="id">The artwork's unique identifier.</param>
    /// <param name="dto">The new display status.</param>
    /// <returns>The artwork with its updated display status.</returns>
    /// <response code="200">Display status updated successfully.</response>
    /// <response code="404">Artwork not found.</response>
    /// <response code="400">Validation failed.</response>
    [HttpPatch("{id:guid}/display-status")]
    [ProducesResponseType(typeof(ArtworkDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ArtworkDto>> PatchDisplayStatus(Guid id, [FromBody] PatchDisplayStatusDto dto)
    {
        var updated = await _service.SetDisplayStatusAsync(id, dto.IsOnDisplay);
        return Ok(updated);
    }
}
