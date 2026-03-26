using System.Linq.Expressions;
using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Common;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.Services;

public class MedicalReviewCommandService : IMedicalReviewCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public MedicalReviewCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<CreateMedicalReviewDto> CreateAsync(CreateMedicalReviewDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var userId = _userContextService.GetUserId();

        var medicalReviewer = await _unitOfWork.GetRepository<MedicalReviewer>()
                                        .FirstOrDefaultAsync(sr => sr.UserId == userId);

        if (medicalReviewer == null)
            throw new UnauthorizedAccessException("User is not a medical reviewer.");

        var report = await _unitOfWork.GetRepository<AefiReport>()
                                .GetByIdAsync(dto.AefiReportId);

        if (report == null)
            throw new KeyNotFoundException("Aefi Report not found.");


        var medicalReview = _mapper.Map<MedicalReview>(dto);
        medicalReview.MedicalReviewer = medicalReviewer;
        medicalReview.MedicalReviewerId = medicalReviewer.Id;

        await _unitOfWork.GetRepository<MedicalReview>().CreateAsync(medicalReview);
        await _unitOfWork.CompleteAsync();

        return dto;
    }

    public async Task<CreateMedicalReviewDto> UpdateAsync(CreateMedicalReviewDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Report DTO cannot be null.");

            // TODO: Implement update logic with proper validation
            await _unitOfWork.CompleteAsync();
            return dto;
        }
        catch (ArgumentNullException ex)
        {
            throw new InvalidOperationException($"Validation error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while updating the report: {ex.Message}", ex);
        }
    }
    public async Task DeleteAsync(int medicalReviewId)
    {
        try
        {
            if (medicalReviewId <= 0)
                throw new ArgumentException("Medical Review ID must be greater than zero.", nameof(medicalReviewId));

            var report = await _unitOfWork.GetRepository<MedicalReview>().GetByIdAsync(medicalReviewId);
            if (report == null)
                throw new KeyNotFoundException($"Medical Review with ID {medicalReviewId} does not exist.");

            await _unitOfWork.GetRepository<MedicalReview>().DeleteByIdAsync(medicalReviewId);
            await _unitOfWork.CompleteAsync();
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException($"Validation error: {ex.Message}", ex);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while deleting the report: {ex.Message}", ex);
        }
    }
}