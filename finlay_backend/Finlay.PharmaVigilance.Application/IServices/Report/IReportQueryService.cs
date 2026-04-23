using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IReportQueryService : IGenericQueryService<AefiReport, PublicAefiReportDto>
{
    Task<ReportResponseDto> GetReportByNotificationNumber(string notificationNumber);
    Task<IEnumerable<ReportResponseDto>> GetReportAssigment();
}