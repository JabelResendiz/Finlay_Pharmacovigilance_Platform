using AutoMapper;
using Finlay.PharmaVigilance.Application.Authentication;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.Services.Authentication;

/// <summary>
/// Service implementation for managing Section Responsible registration and authentication.
/// </summary>
public class SectionResponsibleService : ISectionResponsibleService
{
    private readonly IIdentityManager _identityManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the SectionResponsibleService class.
    /// </summary>
    public SectionResponsibleService(
        IIdentityManager identityManager,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _identityManager = identityManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Registers a new Section Responsible user with their profile information.
    /// </summary>
    public async Task<string> RegisterSectionResponsibleAsync(RegisterSectionResponsibleDto registerDto)
    {
        // Validate inputs
        if (registerDto == null)
            throw new ArgumentNullException(nameof(registerDto), "Registration DTO cannot be null.");

        // Validate that province exists
        var province = await _unitOfWork.GetRepository<Province>().GetByIdAsync(registerDto.ProvinceId);
        if (province == null)
            throw new KeyNotFoundException($"Province with ID {registerDto.ProvinceId} does not exist.");

        // Create User account
        var user = new User
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            UserRole = UserRole.SectionResponsible.ToString()
        };

        var createdUser = await _identityManager.CreateUserAsync(user, registerDto.Password);
        if (createdUser == null)
            throw new InvalidOperationException("Failed to create user account.");

        // Assign SectionResponsible role
        await _identityManager.AddRoles(createdUser.Id.ToString(), UserRole.SectionResponsible.ToString());

        // Create SectionResponsible profile
        var sectionResponsible = new SectionResponsible
        {
            UserId = createdUser.Id,
            ProvinceId = registerDto.ProvinceId
        };

        // Add to repository and save
        await _unitOfWork.GetRepository<SectionResponsible>().CreateAsync(sectionResponsible);
        await _unitOfWork.CompleteAsync();

        return "Section Responsible successfully registered";
    }
}
