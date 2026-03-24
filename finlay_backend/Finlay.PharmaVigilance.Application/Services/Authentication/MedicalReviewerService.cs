using AutoMapper;
using Finlay.PharmaVigilance.Application.Authentication;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services.Authentication;

/// <summary>
/// Service implementation for managing Medical Reviewer registration and authentication.
/// </summary>
public class MedicalReviewerService : IMedicalReviewerService
{
    private readonly IIdentityManager _identityManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IMedicalReviewerRepository _medical;

    /// <summary>
    /// Initializes a new instance of the MedicalReviewerService class.
    /// </summary>
    public MedicalReviewerService(
        IIdentityManager identityManager,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContextService userContextService,
        IMedicalReviewerRepository medical)
    {
        _identityManager = identityManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
        _medical = medical;
    }

    /// <summary>
    /// Registers a new Medical Reviewer user with their profile information.
    /// </summary>
    public async Task<string> RegisterMedicalReviewerAsync(RegisterMedicalReviewerDto registerDto)
    {
        // Validate inputs
        if (registerDto == null)
            throw new ArgumentNullException(nameof(registerDto), "Registration DTO cannot be null.");

        var userId = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (sectionResponsible == null)
            throw new UnauthorizedAccessException("User is not a section responsible.");

        var provinceId = sectionResponsible.ProvinceId;

        // Validate that municipality exists and belongs to the province
        var municipality = await _unitOfWork.GetRepository<Municipality>().GetByIdAsync(registerDto.MunicipalityId);
        if (municipality == null || municipality.ProvinceId != provinceId)
            throw new KeyNotFoundException($"Municipality with ID {registerDto.MunicipalityId} does not exist or does not belong to the specified province.");

        var user = _mapper.Map<User>(registerDto);
        user.UserRole = UserRole.MedicalReviewer.ToString();

        var createdUser = await _identityManager.CreateUserAsync(user, registerDto.Password);
        if (createdUser == null)
            throw new InvalidOperationException("Failed to create user account.");

        // Assign MedicalReviewer role
        await _identityManager.AddRoles(createdUser.Id.ToString(), UserRole.MedicalReviewer.ToString());

        var medicalReviewer = _mapper.Map<MedicalReviewer>(registerDto);
        medicalReviewer.UserId = createdUser.Id;
        medicalReviewer.ProvinceId = provinceId;

        // Add to repository and save
        await _unitOfWork.GetRepository<MedicalReviewer>().CreateAsync(medicalReviewer);
        await _unitOfWork.CompleteAsync();

        return "Medical Reviewer successfully registered";
    }


    public async Task<IEnumerable<GetMedicalReviewerDto>> ListByMunicipalityAsync(int municipalityId, CancellationToken cancellationToken = default)
    {
        if (municipalityId <= 0 || municipalityId >= 16)
            throw new InvalidOperationException($"Invalid input {municipalityId}");

        var userId = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (sectionResponsible == null)
            throw new UnauthorizedAccessException("User is not a section responsible.");

        var provinceId = sectionResponsible.ProvinceId;

        var municipality = await _unitOfWork.GetRepository<Municipality>().GetByIdAsync(municipalityId);
        if (municipality == null || municipality.ProvinceId != provinceId)
            throw new KeyNotFoundException($"Municipality with ID {municipalityId} does not exist or does not belong to the specified province.");

        var medicalList = await _medical.GetByMunicipalityAsync(municipalityId);

        return _mapper.Map<IEnumerable<GetMedicalReviewerDto>>(medicalList);
    }


    public async Task<IEnumerable<GetMedicalReviewerDto>> ListByProvinceAsync(CancellationToken cancellationToken = default)
    {
        var userId = _userContextService.GetUserId();

        var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (sectionResponsible == null)
            throw new UnauthorizedAccessException("User is not a section responsible.");

        var provinceId = sectionResponsible.ProvinceId;

        var medicalList = await _medical.GetByProvinceAsync(provinceId);

        return _mapper.Map<IEnumerable<GetMedicalReviewerDto>>(medicalList);
    }


}
