using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.Api.Controllers;

/// <summary>
/// API Controller responsible for managing Medical Reviewer user operations.
/// Provides endpoints for registration of Medical Reviewer users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MedicalReviewerController : ControllerBase
{
    private readonly IMedicalReviewerService _medicalReviewerService;

    /// <summary>
    /// Initializes a new instance of the MedicalReviewerController class.
    /// </summary>
    public MedicalReviewerController(IMedicalReviewerService medicalReviewerService)
    {
        _medicalReviewerService = medicalReviewerService;
    }

    /// <summary>
    /// Registers a new Medical Reviewer user with their profile information.
    /// </summary>
    /// <param name="registerDto">The DTO containing registration and profile details.</param>
    /// <returns>A response indicating successful registration.</returns>
    /// <response code="200">Medical Reviewer successfully registered.</response>
    /// <response code="400">Bad request - validation failed.</response>
    /// <response code="409">Conflict - user email or username already exists.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterMedicalReviewer(RegisterMedicalReviewerDto registerDto)
    {
        try
        {
            var result = await _medicalReviewerService.RegisterMedicalReviewerAsync(registerDto);

            return Ok(new
            {
                message = result,
                success = true
            });
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { message = ex.Message, success = false });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message, success = false });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message, success = false });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message, success = false });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = $"An error occurred while registering the Medical Reviewer: {ex.Message}", success = false });
        }
    }


}