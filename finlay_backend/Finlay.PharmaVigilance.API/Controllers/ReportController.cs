using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportQueryService _reportQueryService;
    private readonly IReportCommandService _reportCommandService;

    public ReportController(IReportQueryService reportQueryService,
                            IReportCommandService reportCommandService)
    {
        _reportQueryService = reportQueryService;
        _reportCommandService = reportCommandService;
    }

    [HttpPost("createPublic")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePublicReport([FromBody] PublicAefiReportDto reportDto)
    {
        if (reportDto == null)
            throw new ArgumentNullException(nameof(reportDto), "Report data is required.");

        var result = await _reportCommandService.CreatePublicReportAsync(reportDto);

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Report successfully created",
            data = result
        });
    }


    [HttpPost("createMedical")]
    [Authorize(Roles = "Medical Reviewer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateMedicalReport([FromBody] MedicalReportDto reportDto)
    {
        if (reportDto == null)
            throw new ArgumentNullException(nameof(reportDto), "Report data is required.");

        var result = await _reportCommandService.CreateMedicalReportAsync(reportDto);

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Report successfully created",
            data = result
        });
    }



    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<ReportDto>>> GetAllReports()
    // {
    //     var reports = await _reportQueryService.ListAsync();
    //     return Ok(new { message = "Reports retrieved successfully.", data = reports, count = reports.Count() });
    // }

    // [HttpGet("{reportId:int}")]
    // public async Task<ActionResult<ReportDto>> GetReportById(int reportId)
    // {
    //     if (reportId <= 0)
    //         throw new ArgumentException("Report ID must be a valid positive number.");

    //     var report = await _reportQueryService.GetByIdAsync(reportId);

    //     if (report == null)
    //         throw new KeyNotFoundException($"Report with ID {reportId} was not found.");

    //     return Ok(new { message = "Report retrieved successfully.", data = report });
    // }

    // [HttpPut("{reportId:int}")]
    // public async Task<IActionResult> UpdateReport(int reportId, [FromBody] ReportDto reportDto)
    // {
    //     if (reportId <= 0)
    //         throw new ArgumentException("Report ID must be a valid positive number.");

    //     if (reportDto == null)
    //         throw new ArgumentNullException(nameof(reportDto), "Report data is required.");

    //     var existingReport = await _reportQueryService.GetByIdAsync(reportId);
    //     if (existingReport == null)
    //         throw new KeyNotFoundException($"Report with ID {reportId} was not found.");

    //     var result = await _reportCommandService.UpdateAsync(reportDto);

    //     return Ok(new { message = "Report successfully updated.", data = result });
    // }

    // [HttpDelete("{reportId:int}")]
    // public async Task<IActionResult> DeleteReport(int reportId)
    // {
    //     if (reportId <= 0)
    //         throw new ArgumentException("Report ID must be a valid positive number.");

    //     await _reportCommandService.DeleteAsync(reportId);

    //     return NoContent();
    // }


}