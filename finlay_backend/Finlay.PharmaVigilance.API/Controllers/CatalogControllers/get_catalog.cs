
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.Api.Controllers.CatalogControllers;

/// <summary>
/// API Controller responsible for managing Medical Reviewer user operations.
/// Provides endpoints for registration of Medical Reviewer users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GetCatalogController : ControllerBase
{
    private readonly IVaccineQueryService _vaccineQueryService;
    private readonly ISymptomQueryService _symptomsQueryService;

    /// <summary>
    /// Initializes a new instance of the CatalogController class.
    /// </summary>
    public GetCatalogController(
        IVaccineQueryService vaccineQueryService,
        ISymptomQueryService symptomQueryService)
    {
        _vaccineQueryService = vaccineQueryService;
        _symptomsQueryService = symptomQueryService;
    }


    [HttpGet("vaccines")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetVaccines([FromQuery] PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _vaccineQueryService.GetAllPagedResultAsync(paged);

        return Ok(new
        {
            message = result,
            success = true
        });

    }

    [HttpGet("symptoms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetSymptoms([FromQuery] PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _symptomsQueryService.GetAllPagedResultAsync(paged);

        return Ok(new
        {
            message = result,
            success = true
        });

    }




}