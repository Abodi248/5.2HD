using AboriginalArtGallery.API.Models;
using AboriginalArtGallery.Application.Tribes;
using Microsoft.AspNetCore.Mvc;

namespace AboriginalArtGallery.API.Controllers;

/// <summary>Manages Aboriginal tribe reference data.</summary>
[ApiController]
[Route("api/tribes")]
public class TribesController : ControllerBase
{
    private readonly TribeService _service;

    public TribesController(TribeService service)
    {
        _service = service;
    }

    /// <summary>Returns a paginated list of tribes.</summary>
    /// <param name="search">Optional name search term (case-insensitive partial match).</param>
    /// <param name="page">Page number, 1-based (default: 1).</param>
    /// <param name="pageSize">Items per page, max 100 (default: 20).</param>
    /// <returns>A paginated envelope containing matching tribes.</returns>
    /// <response code="200">Returns the paginated tribe list.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TribeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<TribeDto>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, totalCount) = await _service.GetAllAsync(search, page, pageSize);

        return Ok(new PagedResult<TribeDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    /// <summary>Returns a single tribe by ID.</summary>
    /// <param name="id">The tribe's unique identifier.</param>
    /// <returns>The tribe with the specified ID.</returns>
    /// <response code="200">Returns the tribe.</response>
    /// <response code="404">Tribe not found or has been deactivated.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TribeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TribeDto>> GetById(Guid id)
    {
        var tribe = await _service.GetByIdAsync(id);
        return Ok(tribe);
    }

    /// <summary>Creates a new tribe.</summary>
    /// <param name="dto">The tribe data to create.</param>
    /// <returns>The newly created tribe, with a Location header pointing to the resource.</returns>
    /// <response code="201">Tribe created successfully.</response>
    /// <response code="400">Validation failed.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TribeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TribeDto>> Create([FromBody] CreateTribeDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Updates an existing tribe.</summary>
    /// <param name="id">The tribe's unique identifier.</param>
    /// <param name="dto">The replacement tribe data.</param>
    /// <returns>The updated tribe.</returns>
    /// <response code="200">Tribe updated successfully.</response>
    /// <response code="404">Tribe not found.</response>
    /// <response code="400">Validation failed.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TribeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TribeDto>> Update(Guid id, [FromBody] UpdateTribeDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return Ok(updated);
    }

    /// <summary>Soft deletes a tribe by setting is_active to false.</summary>
    /// <param name="id">The tribe's unique identifier.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Tribe deactivated successfully.</response>
    /// <response code="404">Tribe not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
