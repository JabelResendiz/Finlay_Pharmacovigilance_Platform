using Finlay.PharmaVigilance.Application.DTO;


namespace Finlay.PharmaVigilance.Application.Services;


public interface IPdfService
{
    byte[] GenerateReportPdf(ReportPdfDto report);
}