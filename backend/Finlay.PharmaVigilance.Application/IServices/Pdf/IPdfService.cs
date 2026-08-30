using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.Enum;

namespace Finlay.PharmaVigilance.Application.IServices.Pdf;

public interface IPdfService
{
    byte[] GenerateReportPdf(
        ReportUserDto report);
}