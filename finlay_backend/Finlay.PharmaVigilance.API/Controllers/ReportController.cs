// using Finlay.PharmaVigilance.Application.DTO;
// using Finlay.PharmaVigilance.Application.IServices;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace Finlay.PharmaVigilance.Api.Controllers;

// /// <summary>
// /// API Controller responsible for managing system reports.
// /// Provides CRUD operations intended for administrators.
// /// </summary>
// [ApiController]
// [Route("api/[controller]")]
// //[Authorize] // Requires authentication for all endpoints
// public class ReportController : ControllerBase
// {
//     private readonly IReportQueryService _reportQueryService;
//     private readonly IReportCommandService _reportCommandService;

//     public ReportController(IReportQueryService reportQueryService,
//                           IReportCommandService reportCommandService)
//     {
//         _reportQueryService = reportQueryService;
//         _reportCommandService = reportCommandService;
//     }

//     [HttpPost]
//     [Route("POST")]
//     public async Task<IActionResult> CreateReport(ReportDto reportDto)
//     {
//         try
//         {
//             await _reportCommandService.CreateAsync(reportDto);

//             return Ok(new { message = "Successful creation report" }); // 204 - Successful deletion with no content
//         }
//         catch (KeyNotFoundException ex)
//         {
//             return NotFound(new { message = ex.Message });
//         }
//         catch (Exception ex)
//         {
//             return BadRequest(new { message = $"An error occurred while creating the report: {ex.Message}" });
//         }
//     }

//     /// <summary>
//     /// Retrieves all reports in the system.
//     /// Requires Administrator role.
//     /// </summary>
//     /// <returns>A list of all reports.</returns>
//     /// <response code="200">Returns the list of reports.</response>
//     /// <response code="401">Unauthorized - authentication required.</response>
//     /// <response code="403">Forbidden - requires administrator role.</response>
//     [HttpGet]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status403Forbidden)]
//     public async Task<ActionResult<IEnumerable<ReportDto>>> GetAllReports()
//     {
//         var reports = await _reportQueryService.ListAsync();
//         return Ok(reports);
//     }

//     /// <summary>
//     /// Retrieves a specific report by their ID.
//     /// Only administrators can access other reports' information.
//     /// Regular reports can only access their own information.
//     /// </summary>
//     /// <param name="reportId">The ID of the report to retrieve.</param>
//     /// <returns>The requested report's information.</returns>
//     /// <response code="200">Report found.</response>
//     /// <response code="401">Unauthorized.</response>
//     /// <response code="403">Forbidden - you do not have permission to access this report.</response>
//     /// <response code="404">Report not found.</response>
//     [HttpGet("{reportId:int}")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status403Forbidden)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<ActionResult<ReportDto>> GetReportById(int reportId)
//     {
//         var report = await _reportQueryService.GetByIdAsync(reportId);

//         if (report == null)
//             return NotFound(new { message = $"Report with ID {reportId} was not found." });

//         return Ok(report);
//     }

//     /// <summary>
//     /// Deletes a report from the system.
//     /// Only administrators are allowed to delete reports.
//     /// </summary>
//     /// <param name="reportId">The ID of the report to delete.</param>
//     /// <returns>Deletion confirmation.</returns>
//     /// <response code="204">Report successfully deleted.</response>
//     /// <response code="400">Bad request - invalid ID or deletion not allowed.</response>
//     /// <response code="401">Unauthorized.</response>
//     /// <response code="403">Forbidden - requires administrator role.</response>
//     /// <response code="404">Report not found.</response>
//     [HttpDelete("{reportId:int}")]
//     //[Authorize(Roles = "Supervisor")]
//     [ProducesResponseType(StatusCodes.Status204NoContent)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status403Forbidden)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<IActionResult> DeleteReport(int reportId)
//     {
//         try
//         {
//             if (reportId <= 0)
//                 return BadRequest(new { message = "Report ID must be a valid positive number." });

//             await _reportCommandService.DeleteAsync(reportId);

//             return NoContent(); // 204 - Successful deletion with no content
//         }
//         catch (KeyNotFoundException ex)
//         {
//             return NotFound(new { message = ex.Message });
//         }
//         catch (Exception ex)
//         {
//             return BadRequest(new { message = $"An error occurred while deleting the report: {ex.Message}" });
//         }
//     }
// }