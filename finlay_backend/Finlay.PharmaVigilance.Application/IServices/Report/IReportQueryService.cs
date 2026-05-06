using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IReportQueryService : IGenericQueryService<AefiReport, PublicAefiReportDto>
{
    Task<ReportUserDto> GetReportByNotificationNumber(string notificationNumber);

    Task<PagedResultDto<ReportMedicalReviewerDto>> GetReportAssigment(PagedRequestDto paged);

    Task<PagedResultDto<ReportSectionResponsibleDto>> GetReportsBySectionResponsible(PagedRequestDto pagedRequestDto);

    Task<byte[]> GetReportPdfAsync(string notificationNumber);

    Task<PagedResultDto<ReportAdminDto>> GetFilter(
        PagedRequestDto paged,
        string? vaccineName,
        string? provinceName
    );
}