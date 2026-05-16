
using System.Diagnostics;
using System.Text;
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


    public byte[] GenerateReportDetailsPdf(ReportDetailAdminDto report)
    {
        var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "admin-report.tex");

        var texContent = File.ReadAllText(templatePath);

        // =========================
        // 1. Replace simple fields
        // =========================
        texContent = texContent
            .Replace("{{REPORT-DATE}}", DateTime.Now.ToString("yyyy-MM-dd"))
            .Replace("{{NOTIFICATION-NUMBER}}", report.NotificationNumber)
            .Replace("{{STATUS}}", report.Status.ToString())
            .Replace("{{SEVERITY}}", report.GlobalSeverityLevel.ToString())
            .Replace("{{AGE}}", report.VaccinatedSubject.Age.ToString())
            .Replace("{{GENDER}}", report.VaccinatedSubject.Gender.ToString())
            .Replace("{{PROVINCE}}", report.VaccinatedSubject.ProvinceName)
            .Replace("{{PREGNANT}}", report.VaccinatedSubject.IsPregnant != null ? (report.VaccinatedSubject.IsPregnant.Value ? "Sí" : "No") : "No")
            .Replace("{{MEDICAL-HISTORY}}", report.VaccinatedSubject.MedicalHistory ?? "N/A")
            .Replace("{{MEDICATIONS}}", report.VaccinatedSubject.CurrentMedications ?? "N/A")
            .Replace("{{ALLERGIES}}", report.VaccinatedSubject.Allergies ?? "N/A");

        // =========================
        // 2. Build VACCINATIONS block
        // =========================
        var vaccinationsBuilder = new StringBuilder();

        foreach (var v in report.Vaccinations)
        {
            vaccinationsBuilder.AppendLine(@$"
\begin{{tabularx}}{{\textwidth}}{{X X}}
\textbf{{Vacuna}} & {Escape(v.VaccineName)} \\
\textbf{{Lote}} & {Escape(v.LotNumber)} \\
\textbf{{Dosis}} & {v.DoseNumber} \\
\textbf{{Sitio}} & {Escape(v.AdministrationSite.ToString())} \\
\textbf{{Fecha}} & {v.AdministrationDate:yyyy-MM-dd} \\
\textbf{{Centro}} & {Escape(v.VaccinationCenterName)} \\
\end{{tabularx}}
\vspace{{5mm}}
");
        }

        texContent = texContent.Replace("{{VACCINATIONS}}", vaccinationsBuilder.ToString());

        // =========================
        // 3. Build ADVERSE EVENTS
        // =========================
        var eventsBuilder = new StringBuilder();

        foreach (var e in report.AdverseEvents)
        {
            eventsBuilder.AppendLine(@$"
\textbf{{Síntoma}}: {Escape(e.Symptom)}\\
\textbf{{Descripción}}: {Escape(e.Description)}\\
\textbf{{Severidad}}: {Escape(e.SeverityLevel.ToString())}\\
\textbf{{Estado}}: {Escape(e.CurrentStatus.ToString())}\\
\vspace{{3mm}}
\hrule
\vspace{{3mm}}
");
        }

        texContent = texContent.Replace("{{ADVERSE-EVENTS}}", eventsBuilder.ToString());

        // =========================
        // 4. Write temp .tex file
        // =========================
        var tempDir = Path.Combine(Path.GetTempPath(), "finlay-latex");
        Directory.CreateDirectory(tempDir);

        var texFile = Path.Combine(tempDir, $"report-{Guid.NewGuid()}.tex");
        var pdfFile = Path.ChangeExtension(texFile, ".pdf");

        File.WriteAllText(texFile, texContent, Encoding.UTF8);

        // =========================
        // 5. Compile using Docker LaTeX
        // =========================
        var psi = new ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $@"run --rm -v {tempDir}:/workdir texlive/texlive latexmk -pdf report.tex",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = Process.Start(psi);
        process.WaitForExit();

        if (!File.Exists(pdfFile))
            throw new Exception("LaTeX compilation failed");

        return File.ReadAllBytes(pdfFile);
    }

    private string Escape(string input)
    {
        if (string.IsNullOrEmpty(input)) return "N/A";

        return input
            .Replace("\\", @"\textbackslash{}")
            .Replace("&", @"\&")
            .Replace("%", @"\%")
            .Replace("$", @"\$")
            .Replace("#", @"\#")
            .Replace("_", @"\_")
            .Replace("{", @"\{")
            .Replace("}", @"\}");
    }

}