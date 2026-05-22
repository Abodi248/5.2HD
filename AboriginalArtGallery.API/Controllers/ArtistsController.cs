using AboriginalArtGallery.API.Models;
using AboriginalArtGallery.Application.Artists;
using AboriginalArtGallery.Application.Artworks;
using Microsoft.AspNetCore.Mvc;

namespace AboriginalArtGallery.API.Controllers;

/// <summary>Manages artists and their associated artworks in the Aboriginal Art Gallery.</summary>
[ApiController]
[Route("api/artists")]
public class ArtistsController : ControllerBase
{
    private readonly ArtistService _service;

    public ArtistsController(ArtistService service)
    {
        _service = service;
    }

    /// <summary>Returns a paginated list of artists.</summary>
    /// <param name="search">Optional name search term (case-insensitive partial match).</param>
    /// <param name="tribeId">Optional tribe ID to filter by.</param>
    /// <param name="isVerified">Filter by gallery verification status.</param>
    /// <param name="page">Page number, 1-based (default: 1).</param>
    /// <param name="pageSize">Items per page, max 100 (default: 20).</param>
    /// <returns>A paginated envelope containing matching artists.</returns>
    /// <response code="200">Returns the paginated artist list.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ArtistDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ArtistDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] Guid? tribeId,
        [FromQuery] bool? isVerified,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var filters = new ArtistFilterParams
        {
            Search = search,
            TribeId = tribeId,
            IsVerified = isVerified,
            Page = page,
            PageSize = pageSize
        };

        var (items, totalCount) = await _service.GetAllAsync(filters);

        return Ok(new PagedResult<ArtistDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    /// <summary>Returns a single artist by ID.</summary>
    /// <param name="id">The artist's unique identifier.</param>
    /// <returns>The artist with the specified ID.</returns>
    /// <response code="200">Returns the artist.</response>
    /// <response code="404">Artist not found or has been deactivated.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ArtistDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArtistDto>> GetById(Guid id)
    {
        var artist = await _service.GetByIdAsync(id);
        return Ok(artist);
    }

    /// <summary>Returns all artworks created by this artist.</summary>
    /// <param name="id">The artist's unique identifier.</param>
    /// <returns>A flat list of active artworks associated with the artist.</returns>
    /// <response code="200">Returns the artworks.</response>
    /// <response code="404">Artist not found.</response>
    [HttpGet("{id:guid}/artworks")]
    [ProducesResponseType(typeof(IEnumerable<ArtworkDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ArtworkDto>>> GetArtworks(Guid id)
    {
        var artworks = await _service.GetArtworksByArtistIdAsync(id);
        return Ok(artworks);
    }

    /// <summary>Returns aggregate statistics for an artist.</summary>
    /// <param name="id">The artist's unique identifier.</param>
    /// <returns>Statistics including artwork count, year range, and distinct media used.</returns>
    /// <response code="200">Returns the artist summary.</response>
    /// <response code="404">Artist not found.</response>
    [HttpGet("{id:guid}/summary")]
    [ProducesResponseType(typeof(ArtistSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArtistSummaryDto>> GetSummary(Guid id)
    {
        var summary = await _service.GetSummaryAsync(id);
        return Ok(summary);
    }

    /// <summary>Creates a new artist.</summary>
    /// <param name="dto">The artist data to create.</param>
    /// <returns>The newly created artist, with a Location header pointing to the resource.</returns>
    /// <response code="201">Artist created successfully.</response>
    /// <response code="400">Validation failed or a domain rule was violated.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ArtistDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ArtistDto>> Create([FromBody] CreateArtistDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Updates an existing artist.</summary>
    /// <param name="id">The artist's unique identifier.</param>
    /// <param name="dto">The replacement artist data.</param>
    /// <returns>The updated artist.</returns>
    /// <response code="200">Artist updated successfully.</response>
    /// <response code="404">Artist not found.</response>
    /// <response code="400">Validation failed or a domain rule was violated.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ArtistDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ArtistDto>> Update(Guid id, [FromBody] UpdateArtistDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return Ok(updated);
    }

    /// <summary>Soft deletes an artist by setting is_active to false.</summary>
    /// <param name="id">The artist's unique identifier.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Artist deactivated successfully.</response>
    /// <response code="404">Artist not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
