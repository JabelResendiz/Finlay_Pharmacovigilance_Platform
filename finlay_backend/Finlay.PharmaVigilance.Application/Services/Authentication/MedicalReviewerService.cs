using AutoMapper;
using Finlay.PharmaVigilance.Application.Authentication;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.Services.Authentication;

/// <summary>
/// Service implementation for managing Medical Reviewer registration and authentication.
/// </summary>
public class MedicalReviewerService : IMedicalReviewerService
{
    private readonly IIdentityManager _identityManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the MedicalReviewerService class.
    /// </summary>
    public MedicalReviewerService(
        IIdentityManager identityManager,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _identityManager = identityManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Registers a new Medical Reviewer user with their profile information.
    /// </summary>
    public async Task<string> RegisterMedicalReviewerAsync(RegisterMedicalReviewerDto registerDto)
    {
        // Validate inputs
        if (registerDto == null)
            throw new ArgumentNullException(nameof(registerDto), "Registration DTO cannot be null.");

        if (string.IsNullOrWhiteSpace(registerDto.Email))
            throw new ArgumentException("Email is required.", nameof(registerDto.Email));

        if (string.IsNullOrWhiteSpace(registerDto.UserName))
            throw new ArgumentException("Username is required.", nameof(registerDto.UserName));

        if (string.IsNullOrWhiteSpace(registerDto.Password) || registerDto.Password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long.", nameof(registerDto.Password));

        // Validate that province exists
        var province = await _unitOfWork.GetRepository<Province>().GetByIdAsync(registerDto.ProvinceId);
        if (province == null)
            throw new KeyNotFoundException($"Province with ID {registerDto.ProvinceId} does not exist.");

        // Validate that municipality exists and belongs to the province
        var municipality = await _unitOfWork.GetRepository<Municipality>().GetByIdAsync(registerDto.MunicipalityId);
        if (municipality == null || municipality.ProvinceId != registerDto.ProvinceId)
            throw new KeyNotFoundException($"Municipality with ID {registerDto.MunicipalityId} does not exist or does not belong to the specified province.");

        // Create User account
        var user = new User
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            UserRole = UserRole.MedicalReviewer.ToString()
        };

        var createdUser = await _identityManager.CreateUserAsync(user, registerDto.Password);
        if (createdUser == null)
            throw new InvalidOperationException("Failed to create user account.");

        // Assign MedicalReviewer role
        await _identityManager.AddRoles(createdUser.Id.ToString(), UserRole.MedicalReviewer.ToString());

        // Create MedicalReviewer profile
        var medicalReviewer = new MedicalReviewer
        {
            UserId = createdUser.Id,
            FullName = registerDto.Name,
            DateOfBirth = registerDto.DateOfBirth,
            Gender = registerDto.Gender,
            ProvinceId = registerDto.ProvinceId,
            MunicipalityId = registerDto.MunicipalityId,
            HealthArea = registerDto.HealthArea,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Add to repository and save
        await _unitOfWork.GetRepository<MedicalReviewer>().CreateAsync(medicalReviewer);
        await _unitOfWork.CompleteAsync();

        return "Medical Reviewer successfully registered";
    }
}
