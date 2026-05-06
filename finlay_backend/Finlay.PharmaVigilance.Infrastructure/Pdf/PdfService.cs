
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.Services;
using iText.Forms;
using iText.Kernel.Pdf;

namespace Finlay.PharmaVigilance.Infrastructure.Pdf;

public class PdfService : IPdfService
{
    public byte[] GenerateReportPdf(ReportPdfDto report)
    {
        using var ms = new MemoryStream();

        var path = Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory,
    "Templates",
    "report-template.pdf"
);

        var reader = new PdfReader(path);

        //var reader = new PdfReader("Templates/report-template.pdf");

        var writer = new PdfWriter(ms);

        var pdfDoc = new PdfDocument(reader, writer);

        // var form = PdfAcroForm.GetAcroForm(pdfDoc, true);

        var form = PdfAcroForm.GetAcroForm(pdfDoc, false);

        foreach (var field in form.GetAllFormFields())
        {
            Console.WriteLine(field.Key);
        }

        form.GetField("nombre_reportante")?.SetValue(report.Reporter.FullName);
        form.GetField("email")?.SetValue(report.Reporter.Email);
        // form.GetField("sintomas")?.SetValue(report.A);
        // form.GetField("fecha_evento")?.SetValue(report.EventDate.ToString("yyyy-MM-dd"));

        pdfDoc.Close();

        return ms.ToArray();
    }
}