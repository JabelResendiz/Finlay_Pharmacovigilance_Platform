using System.Linq.Expressions;
using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Application.Services.Report.Validators;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.Services;

/// <summary>
/// Service for managing report command operations (Create, Update, Delete).
/// Handles creation, update, and deletion of AEFI (Adverse Event Following Immunization) reports
/// with comprehensive validation and error handling.
/// 
/// This service uses the Chain of Responsibility pattern with IReportValidator implementations
/// to ensure all business rules are validated before creating a report.
/// </summary>
public class ReportCommandService : IReportCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly INotificationNumberGenerator _generator;
    private readonly IEnumerable<IReportValidator> _validators;

    public ReportCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        INotificationNumberGenerator generator,
        IEnumerable<IReportValidator> validators)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _generator = generator ?? throw new ArgumentNullException(nameof(generator));
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
    }

    public async Task<CreateReportResponseDto> CreatePublicReportAsync(PublicAefiReportDto reportDto)
    {
        if (reportDto == null)
            throw new ArgumentNullException(nameof(reportDto), "Report data is required.");

        try
        {
            // Execute all validators in sequence using Chain of Responsibility pattern
            foreach (var validator in _validators)
            {
                await validator.ValidateAsync(reportDto);
            }

            // Step 1: Get or create VaccinatedSubject (patient)
            var vaccinatedSubjectRepository = _unitOfWork.GetRepository<VaccinatedSubject>();
            var existingVaccinatedSubject = await vaccinatedSubjectRepository
                .FirstOrDefaultAsync(x => x.IdentityNumber == reportDto.VaccinatedSubject.IdentityNumber);

            VaccinatedSubject vaccinatedSubject;
            if (existingVaccinatedSubject != null)
            {
                vaccinatedSubject = existingVaccinatedSubject;
            }
            else
            {
                vaccinatedSubject = _mapper.Map<VaccinatedSubject>(reportDto.VaccinatedSubject);
            }

            // Step 2: Get or create Reporter by normalized full name
            var reporterRepository = _unitOfWork.GetRepository<Reporter>();
            var existingReporter = await reporterRepository
                .FirstOrDefaultAsync(x => x.IdentityNumber == reportDto.Reporter.IdentityNumber);

            Reporter reporter;
            if (existingReporter != null)
            {
                reporter = existingReporter;
            }
            else
            {
                reporter = _mapper.Map<Reporter>(reportDto.Reporter);
            }

            var report = _mapper.Map<AefiReport>(reportDto);
            report.VaccinatedSubjectId = vaccinatedSubject.Id;
            report.VaccinatedSubject = vaccinatedSubject;
            report.ReporterId = reporter.Id;
            report.Reporter = reporter;
            report.Status = ReportStatus.Submitted;


            // foreach (var adverseEventDto in reportDto.AdverseEvents)
            // {

            //     var adverseEventEntity = _mapper.Map<AdverseEvent>(adverseEventDto);

            //     foreach (var symptomId in adverseEventDto.Symptoms)
            //     {
            //         var reportSymptom = new AdverseEventSymptom
            //         {
            //             AdverseEventId = adverseEventEntity.Id,
            //             SymptomId = symptomId,
            //             AdverseEvent = adverseEventEntity,
            //             Symptom = await _unitOfWork.GetRepository<Symptom>().GetByIdAsync(symptomId)
            //                 ?? throw new KeyNotFoundException($"Symptom with ID {symptomId} not found.")
            //         };
            //     }
            // }


            report.NotificationNumber = _generator.Generate();

            await _unitOfWork.GetRepository<AefiReport>().CreateAsync(report);
            await _unitOfWork.CompleteAsync();

            return new CreateReportResponseDto
            {
                NotificationNumber = report.NotificationNumber
            };

        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Error to create AEFI report: {ex.Message}",
                ex);
        }
    }

    public Task<string> CreateMedicalReportAsync(MedicalReportDto reportDto)
    {
        if (reportDto == null)
            throw new ArgumentNullException(nameof(reportDto), "Medical report data is required.");

        // TODO: Implement medical report creation with validation
        // Medical reports may have different validation rules than public reports

        return Task.FromResult(string.Empty);
    }

    // /// <summary>
    // /// Creates a new AEFI report with validation of all related entities.
    // /// Validates Reporter, VaccinatedSubject, Vaccination, Vaccine, and AdverseEvents before creation.
    // /// </summary>
    // /// <param name="dto">Report data transfer object</param>
    // /// <returns>Created report DTO</returns>
    // /// <exception cref="ArgumentNullException">Thrown when DTO or required nested objects are null</exception>
    // /// <exception cref="ArgumentException">Thrown when required fields are invalid or empty</exception>
    // /// <exception cref="KeyNotFoundException">Thrown when referenced entities don't exist in database</exception>
    // /// <exception cref="InvalidOperationException">Thrown when database operation fails</exception>
    // public async Task<CreateReportResponseDto> CreateAsync(ReportDto dto)
    // {
    //     if (dto == null)
    //         throw new ArgumentNullException(nameof(dto));

    //     // Validate required nested objects
    //     var reporterProvince = await _unitOfWork.GetRepository<Province>()
    //         .GetByIdAsync(dto.Reporter.ProvinceId) ?? throw new KeyNotFoundException($"Province {dto.Reporter.ProvinceId} not found.");

    //     var reporterMunicipality = await _unitOfWork.GetRepository<Municipality>()
    //         .GetByIdAsync(dto.Reporter.MunicipalityId) ?? throw new KeyNotFoundException($"Municipality {dto.Reporter.MunicipalityId} not found.");

    //     if (reporterMunicipality.ProvinceId != reporterProvince.Id)
    //         throw new ArgumentException($"The reporter's municipality {reporterMunicipality.Id} does not belong to province {reporterProvince.Id}.");

    //     var patientProvince = await _unitOfWork.GetRepository<Province>()
    //         .GetByIdAsync(dto.VaccinatedSubject.ProvinceId) ?? throw new KeyNotFoundException($"Province {dto.VaccinatedSubject.ProvinceId} not found.");

    //     var patientMunicipality = await _unitOfWork.GetRepository<Municipality>()
    //         .GetByIdAsync(dto.VaccinatedSubject.MunicipalityId) ?? throw new KeyNotFoundException($"Municipality {dto.VaccinatedSubject.MunicipalityId} not found.");

    //     if (patientMunicipality.ProvinceId != patientProvince.Id)
    //         throw new ArgumentException($"The vaccinated subject's municipality {patientMunicipality.Id} does not   belong to province {patientProvince.Id}.");

    //     if (dto.AdverseEvents == null || !dto.AdverseEvents.Any())
    //         throw new ArgumentException("At least one adverse event is required.", nameof(dto.AdverseEvents));

    //     foreach (var ae in dto.AdverseEvents)
    //     {
    //         if (ae.Symptoms == null || !ae.Symptoms.Any())
    //             throw new ArgumentException("Each adverse event must have at least one symptom.", nameof(ae.Symptoms));
    //     }

    //     // Check if VaccinatedSubject already exists
    //     var vaccinatedRepo = _unitOfWork.GetRepository<VaccinatedSubject>();
    //     var existingSubject = await vaccinatedRepo.FirstOrDefaultAsync(
    //         x => x.IdentityNumber == dto.VaccinatedSubject.IdentityNumber
    //     );

    //     VaccinatedSubject subjectEntity;
    //     if (existingSubject != null)
    //     {
    //         // Use existing
    //         subjectEntity = existingSubject;
    //     }
    //     else
    //     {
    //         // Map and create new
    //         subjectEntity = _mapper.Map<VaccinatedSubject>(dto.VaccinatedSubject);
    //         await vaccinatedRepo.CreateAsync(subjectEntity);
    //     }

    //     // Mapping and save
    //     var report = _mapper.Map<AefiReport>(dto);
    //     report.VaccinatedSubjectId = subjectEntity.Id;
    //     report.VaccinatedSubject = subjectEntity;
    //     report.Status = Domain.Enum.ReportStatus.Submitted;
    //     report.NotificationNumber = _generator.Generate();

    //     // Save report
    //     await _unitOfWork.GetRepository<AefiReport>().CreateAsync(report);
    //     await _unitOfWork.CompleteAsync();

    //     return new CreateReportResponseDto
    //     {
    //         NotificationNumber = report.NotificationNumber
    //     };
    // }
    // /// <summary>
    // /// Updates an existing report (placeholder for future implementation).
    // /// </summary>
    // /// <param name="dto">Updated report DTO</param>
    // /// <returns>Updated report DTO</returns>
    // /// <exception cref="ArgumentNullException">Thrown when DTO is null</exception>
    // /// <exception cref="InvalidOperationException">Thrown when database operation fails</exception>
    // public async Task<ReportDto> UpdateAsync(ReportDto dto)
    // {
    //     try
    //     {
    //         if (dto == null)
    //             throw new ArgumentNullException(nameof(dto), "Report DTO cannot be null.");

    //         // TODO: Implement update logic with proper validation
    //         await _unitOfWork.CompleteAsync();
    //         return dto;
    //     }
    //     catch (ArgumentNullException ex)
    //     {
    //         throw new InvalidOperationException($"Validation error: {ex.Message}", ex);
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new InvalidOperationException($"An error occurred while updating the report: {ex.Message}", ex);
    //     }
    // }

    // /// <summary>
    // /// Deletes a report by ID.
    // /// </summary>
    // /// <param name="reportId">ID of the report to delete</param>
    // /// <exception cref="ArgumentException">Thrown when report ID is invalid</exception>
    // /// <exception cref="KeyNotFoundException">Thrown when report doesn't exist</exception>
    // /// <exception cref="InvalidOperationException">Thrown when database operation fails</exception>
    // public async Task DeleteAsync(int reportId)
    // {
    //     try
    //     {
    //         if (reportId <= 0)
    //             throw new ArgumentException("Report ID must be greater than zero.", nameof(reportId));

    //         var report = await _unitOfWork.GetRepository<AefiReport>().GetByIdAsync(reportId);
    //         if (report == null)
    //             throw new KeyNotFoundException($"Report with ID {reportId} does not exist.");

    //         await _unitOfWork.GetRepository<AefiReport>().DeleteByIdAsync(reportId);
    //         await _unitOfWork.CompleteAsync();
    //     }
    //     catch (ArgumentException ex)
    //     {
    //         throw new InvalidOperationException($"Validation error: {ex.Message}", ex);
    //     }
    //     catch (KeyNotFoundException)
    //     {
    //         throw;
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new InvalidOperationException($"An error occurred while deleting the report: {ex.Message}", ex);
    //     }
    // }
}