using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Finlay.PharmaVigilance.Application.Services;
using Finlay.PharmaVigilance.Application.Services.Authentication;
using Finlay.PharmaVigilance.Application.Services.Report.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finlay.PharmaVigilance.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Adds application-specific services to the dependency injection container 
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddAplication(this IServiceCollection services, ConfigurationManager configurationManager)
    {

        // Registers AutoMapper to enable mapping between DTOs and domain models.
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Email Service
        services.AddScoped<IEmailAppService, EmailAppService>();
        services.AddScoped<IContactCommandService, ContactCommandService>();
        services.AddScoped<IContactQueryService, ContactQueryService>();

        // Catalog Service
        services.AddScoped<ICatalogCommandService, CatalogCommandService>();

        // Registers services related to Entities
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IMedicalReviewerService, MedicalReviewerService>();
        services.AddScoped<ISectionResponsibleService, SectionResponsibleService>();

        // User Services
        services.AddScoped<IUserQueryServices, UserQueryService>();
        services.AddScoped<IUserCommandServices, UserCommandService>();

        // reporter
        services.AddScoped<IReportCommandService, ReportCommandService>();
        services.AddScoped<IReportQueryService, ReportQueryService>();

        // Report Validators - Chain of Responsibility pattern for comprehensive validation
        services.AddScoped<IReportValidator, ReportDateValidator>();
        services.AddScoped<IReportValidator, ReporterValidator>();
        services.AddScoped<IReportValidator, VaccinatedSubjectValidator>();
        services.AddScoped<IReportValidator, VaccinationValidator>();
        services.AddScoped<IReportValidator, AdverseEventValidator>();

        // Notification Number Generator
        services.AddScoped<INotificationNumberGenerator, NotificationNumberGenerator>();

        // Medical review
        services.AddScoped<IMedicalReviewCommandService, MedicalReviewCommandService>();
        services.AddScoped<IMedicalReviewQueryService, MedicalReviewQueryService>();

        services.AddScoped<IMedicalReviewAssignmentCommandService, MedicalReviewAssignmentCommandService>();

        return services;


    }
}