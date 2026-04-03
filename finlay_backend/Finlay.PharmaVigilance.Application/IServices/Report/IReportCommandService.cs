using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IReportCommandService
{
    Task<CreateReportResponseDto> CreatePublicReportAsync(PublicAefiReportDto reportDto);
    Task<string> CreateMedicalReportAsync(MedicalReportDto reportDto);
}