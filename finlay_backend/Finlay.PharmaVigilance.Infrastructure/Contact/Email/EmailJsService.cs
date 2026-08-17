// using System.Reflection;
// using System.Text;
// using System.Text.Json;
// using System.Text.Json.Serialization;
// using Finlay.PharmaVigilance.Application.DTO;
// using Finlay.PharmaVigilance.Application.IServices;
// using Finlay.PharmaVigilance.Domain.Enum;
// using Finlay.PharmaVigilance.Infrastructure.Settings;
// using Microsoft.Extensions.Options;

// namespace Finlay.PharmaVigilance.Infrastructure.Email;

// public class EmailJsService : IEmailService
// {
//     private readonly IHttpClientFactory _httpClientFactory;
//     private readonly EmailJsSettings _settings;
//     private readonly string _logoDataUri;

//     public EmailJsService(
//         IHttpClientFactory httpClientFactory,
//         IOptions<EmailJsSettings> options)
//     {
//         _httpClientFactory = httpClientFactory;
//         _settings = options.Value;
//         _logoDataUri = GetLogoDataUri(); // Se carga una sola vez al instanciar
//     }

//     public async Task SendEmailAsync<T>(
//         string toEmail,
//         EmailTemplateType templateType,
//         T templateData) where T : IBasicTemplate
//     {
//         var url = "https://api.emailjs.com/api/v1.0/email/send";

//         var templateId = GetTemplateId(templateType);
//         var templateParams = ConvertToDictionary(templateData);
//         templateParams["email"] = toEmail;
//         templateParams["logo"] = _logoDataUri;

//         var payload = new
//         {
//             service_id = _settings.ServiceId,
//             template_id = templateId,
//             user_id = _settings.UserId,
//             accessToken = _settings.AccessToken,
//             template_params = templateParams
//         };

//         var json = JsonSerializer.Serialize(payload);
//         var content = new StringContent(json, Encoding.UTF8, "application/json");

//         var client = _httpClientFactory.CreateClient();

//         var response = await client.PostAsync(url, content);

//         if (!response.IsSuccessStatusCode)
//         {
//             var error = await response.Content.ReadAsStringAsync();
//             throw new Exception($"Error en EmailJS: {error}");
//         }
//     }

//     private static string GetLogoDataUri()
//     {
//         string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png");

//         if (!File.Exists(imagePath))
//             throw new FileNotFoundException($"No se encontró logo.png en: {imagePath}");

//         byte[] imageBytes = File.ReadAllBytes(imagePath);
//         return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
//     }


//     private string GetTemplateId(
//        EmailTemplateType templateType)
//     {
//         return templateType switch
//         {
//             EmailTemplateType.ActivateAccount =>
//                 _settings.ActivateAccount,

//             EmailTemplateType.SelfReportConfirmation =>
//                 _settings.SelfReportConfirmation,

//             EmailTemplateType.AssignmentExpired =>
//                 _settings.AssignmentExpired,

//             EmailTemplateType.SectionReportAlert =>
//                 _settings.SectionReportAlert,

//             EmailTemplateType.MedicalReviewerAssignment =>
//                 _settings.MedicalReviewerAssignment,

//             _ => throw new ArgumentOutOfRangeException(
//                 nameof(templateType),
//                 $"Template type is not supported: {templateType}")
//         };
//     }


//     private static Dictionary<string, string>
//      ConvertToDictionary<T>(T templateData)
//     {
//         if (templateData == null)
//         {
//             return new Dictionary<string, string>();
//         }

//         return typeof(T)
//             .GetProperties(
//                 BindingFlags.Public |
//                 BindingFlags.Instance)
//             .ToDictionary(
//                 property =>
//                     property
//                         .GetCustomAttribute<JsonPropertyNameAttribute>()
//                         ?.Name
//                     ?? property.Name,

//                 property =>
//                     property
//                         .GetValue(templateData)?
//                         .ToString()
//                     ?? string.Empty
//             );
//     }
// }



using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Finlay.PharmaVigilance.Infrastructure.Email;

public class EmailJsService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly EmailJsSettings _settings;
    private readonly string _logoDataUri;

    public EmailJsService(
        HttpClient httpClient,
        IOptions<EmailJsSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _logoDataUri = GetLogoDataUri();
    }

    public async Task SendEmailAsync<T>(
        string toEmail,
        EmailTemplateType templateType,
        T templateData)
        where T : IBasicTemplate
    {
        var html = BuildHtml(templateType, templateData, toEmail);


        Console.WriteLine($"HTML generado para {toEmail}:\n{html}\n");

        var templateParams = new Dictionary<string, string>
        {
            { "email", toEmail },
            {"logo", "https://www.finlay.edu.cu/wp-content/themes/extranet-finlay/images/logo.jpg" },
            { "body", html }
        };

        var payload = new
        {
            service_id = _settings.ServiceId,
            template_id = _settings.HtmlTemplateId, // Una sola plantilla de EmailJS
            user_id = _settings.UserId,
            accessToken = _settings.AccessToken,
            template_params = templateParams
        };

        var json = JsonSerializer.Serialize(payload);

        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        // var client = _httpClientFactory.CreateClient();

        var response = await _httpClient.PostAsync(
            "https://api.emailjs.com/api/v1.0/email/send",
            content);

        if (!response.IsSuccessStatusCode)
        {
            var error =
                await response.Content.ReadAsStringAsync();

            throw new Exception(
                $"Error en EmailJS: {error}");
        }
    }

    private static string GetLogoDataUri()
    {
        string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
        "Contact",
        "Email",
        "Templates",
        "logo.png");

        // if (!File.Exists(imagePath))
        //     throw new FileNotFoundException($"No se encontró logo.png en: {imagePath}");

        // byte[] imageBytes = File.ReadAllBytes(imagePath);
        // return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";

        if (!File.Exists(imagePath))
            throw new FileNotFoundException($"No se encontró logo en: {imagePath}");

        byte[] imageBytes = File.ReadAllBytes(imagePath);

        return $"data:image/webp;base64,{Convert.ToBase64String(imageBytes)}";
    }

    private string BuildHtml<T>(
        EmailTemplateType templateType,
        T templateData,
        string email)
    {
        var templatePath = GetTemplatePath(templateType);

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException(
                $"No se encontró la plantilla: {templatePath}");
        }

        var html = File.ReadAllText(templatePath);

        if (templateData != null)
        {
            foreach (var property in typeof(T).GetProperties(
                BindingFlags.Public |
                BindingFlags.Instance))
            {
                var key =
                    property
                        .GetCustomAttribute<JsonPropertyNameAttribute>()
                        ?.Name
                    ?? property.Name;

                var value =
                    property
                        .GetValue(templateData)?
                        .ToString()
                    ?? string.Empty;

                html = html.Replace(
                    $"{{{{{key}}}}}",
                    value);
            }
        }

        html = html.Replace(
            "{{email}}",
            email ?? string.Empty);

        html = html.Replace(
            "{{logo}}",
            _logoDataUri);

        return html;
    }

    private string GetTemplatePath(
        EmailTemplateType templateType)
    {
        var fileName = templateType switch
        {
            EmailTemplateType.ActivateAccount =>
                "Register.html",

            EmailTemplateType.SelfReportConfirmation =>
                "SelfReportConfirmation.html",

            EmailTemplateType.AssignmentExpired =>
                "AssignmentExpired.html",

            EmailTemplateType.SectionReportAlert =>
                "SectionReportAlert.html",

            EmailTemplateType.MedicalReviewerAssignment =>
                "MedicalReviewerAssignment.html",

            _ => throw new ArgumentOutOfRangeException(
                nameof(templateType),
                $"Template type is not supported: {templateType}")
        };

        return Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Contact",
            "Email",
            "Templates",
            fileName);
    }


}