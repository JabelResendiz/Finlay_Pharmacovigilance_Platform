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
            report.NotificationNumber = _generator.Generate();

            var sectionResponsible = await _unitOfWork.GetRepository<SectionResponsible>()
                .FirstOrDefaultAsync(sr => sr.MunicipalityId == reportDto.VaccinatedSubject.MunicipalityId);

            if (sectionResponsible == null)
                throw new InvalidOperationException("No SectionResponsible found.");

            var alert = new Alert
            {
                Description = "New AEFI report submitted",
                IsActive = true,
                IsRead = false,
                ReadAt = null,
                SectionResponsibleId = sectionResponsible.Id
            };

            report.Alerts.Add(alert);

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

}