using AboriginalArtGallery.API.Models;
using AboriginalArtGallery.Application.ArtTypes;
using Microsoft.AspNetCore.Mvc;

namespace AboriginalArtGallery.API.Controllers;

/// <summary>Manages art type reference data for Aboriginal art classification.</summary>
[ApiController]
[Route("api/art-types")]
public class ArtTypesController : ControllerBase
{
    private readonly ArtTypeService _service;

    public ArtTypesController(ArtTypeService service)
    {
        _service = service;
    }

    /// <summary>Returns a paginated list of art types.</summary>
    /// <param name="search">Optional name search term (case-insensitive partial match).</param>
    /// <param name="category">Optional category filter (exact match, e.g. "Painting").</param>
    /// <param name="page">Page number, 1-based (default: 1).</param>
    /// <param name="pageSize">Items per page, max 100 (default: 20).</param>
    /// <returns>A paginated envelope containing matching art types.</returns>
    /// <response code="200">Returns the paginated art type list.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ArtTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ArtTypeDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? category,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, totalCount) = await _service.GetAllAsync(search, category, page, pageSize);

        return Ok(new PagedResult<ArtTypeDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    /// <summary>Returns a single art type by ID.</summary>
    /// <param name="id">The art type's unique identifier.</param>
    /// <returns>The art type with the specified ID.</returns>
    /// <response code="200">Returns the art type.</response>
    /// <response code="404">Art type not found or has been deactivated.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ArtTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArtTypeDto>> GetById(Guid id)
    {
        var artType = await _service.GetByIdAsync(id);
        return Ok(artType);
    }

    /// <summary>Creates a new art type.</summary>
    /// <param name="dto">The art type data to create.</param>
    /// <returns>The newly created art type, with a Location header pointing to the resource.</returns>
    /// <response code="201">Art type created successfully.</response>
    /// <response code="400">Validation failed.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ArtTypeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ArtTypeDto>> Create([FromBody] CreateArtTypeDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Updates an existing art type.</summary>
    /// <param name="id">The art type's unique identifier.</param>
    /// <param name="dto">The replacement art type data.</param>
    /// <returns>The updated art type.</returns>
    /// <response code="200">Art type updated successfully.</response>
    /// <response code="404">Art type not found.</response>
    /// <response code="400">Validation failed.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ArtTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ArtTypeDto>> Update(Guid id, [FromBody] UpdateArtTypeDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return Ok(updated);
    }

    /// <summary>Soft deletes an art type by setting is_active to false.</summary>
    /// <param name="id">The art type's unique identifier.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Art type deactivated successfully.</response>
    /// <response code="404">Art type not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
