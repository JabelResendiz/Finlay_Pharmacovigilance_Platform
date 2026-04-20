// using Finlay.PharmaVigilance.Application.IServices;
// using Finlay.PharmaVigilance.Domain.Events;

// namespace Finlay.PharmaVigilance.Infrastructure.Consumers;

// public class ReportCreatedConsumer
// {
//     private readonly IEmailService _emailService;
//     public ReportCreatedConsumer(IEmailService emailService)
//     {
//         _emailService = emailService;
//     }

//     public async Task Handle(ReportCreatedEvent evt)
//     {
//         await _emailService.SendEmailAsync(
//             evt.ReporterEmail,
//             "Reporte recibido",
//             $"Tu reporte {evt.ReportNumber} fue registrado."
//         );

//         await _emailService.SendEmailAsync(
//             evt.SectionResponsibleEmail,
//             "Nuevo reporte",
//             $"Se registró el reporte {evt.ReportNumber}."
//         );
//     }

// }