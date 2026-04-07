# Project Tree


```
📦finlay_backend
 ┣ 📂Finlay.PharmaVigilance.API
 ┃ ┣ 📂Common
 ┃ ┃ ┗ 📄UserContextService.cs
 ┃ ┣ 📂Controllers
 ┃ ┃ ┣ 📄AuthenticationController.cs
 ┃ ┃ ┗ 📄UserController.cs
 ┃ ┣ 📂Middleware
 ┃ ┃ ┗ 📄ErrorHandlingMiddleware.cs
 ┃ ┣ 📄DependencyInjection.cs
 ┃ ┗ 📄Program.cs
 ┣ 📂Finlay.PharmaVigilance.Application
 ┃ ┣ 📂Authentication
 ┃ ┃ ┗ 📄IIdentityManager.cs
 ┃ ┣ 📂Common
 ┃ ┃ ┗ 📄IJwtTokenGenerator.cs
 ┃ ┣ 📂DTOs
 ┃ ┃ ┣ 📂Authentication
 ┃ ┃ ┃ ┣ 📄LoginUserDto.cs
 ┃ ┃ ┗ 📄AutomapperProfile.cs
 ┃ ┣ 📂IRepository
 ┃ ┃ ┣ 📄IAdverseEventRepository.cs
 ┃ ┃ ┗ 📄IVaccineRepository.cs
 ┃ ┣ 📂IServices
 ┃ ┃ ┣ 📂Authentication
 ┃ ┃ ┃ ┣ 📄IIdentityService.cs
 ┃ ┃ ┃ ┣ 📄IMedicalReviewerService.cs
 ┃ ┃ ┃ ┗ 📄ISectionResponsibleService.cs
 ┃ ┣ 📂IUnitOfWork
 ┃ ┃ ┗ 📄IUnitOfWork.cs
 ┃ ┣ 📂Services
 ┃ ┃ ┣ 📂Authentication
 ┃ ┃ ┃ ┣ 📄IdentityService.cs
 ┃ ┃ ┃ ┣ 📄MedicalReviewerService.cs
 ┃ ┃ ┃ ┗ 📄SectionResponsibleService.cs
 ┃ ┣ 📄DependencyInjection.cs
 ┣ 📂Finlay.PharmaVigilance.Domain
 ┃ ┣ 📂Const
 ┃ ┃ ┣ 📄GenericEntity.cs
 ┃ ┃ ┗ 📄IEntity.cs
 ┃ ┣ 📂Entities
 ┃ ┃ ┣ 📄User.cs
 ┃ ┃ ┣ 📄VaccinatedSubject.cs
 ┃ ┃ ┗ 📄Vaccine.cs
 ┃ ┣ 📂Enum
 ┃ ┃ ┣ 📄UserRole.cs
 ┃ ┃ ┗ 📄VaccineType.cs
 ┣ 📂Finlay.PharmaVigilance.Infrastructure
 ┃ ┣ 📂Authentication
 ┃ ┃ ┣ 📄JwtSettings.cs
 ┃ ┃ ┗ 📄JwtTokenGenerator.cs
 ┃ ┣ 📂Email
 ┃ ┃ ┗ 📄SmtpEmailService.cs
 ┃ ┣ 📂IdentityManager
 ┃ ┃ ┗ 📄IdentityManager.cs
 ┃ ┣ 📂Initializer
 ┃ ┃ ┣ 📄ContextInitializer.cs
 ┃ ┃ ┗ 📄RoleInitializer.cs
 ┃ ┣ 📂Migrations
 ┃ ┣ 📂Repository
 ┃ ┃ ┣ 📄AdverseEventRepository.cs
 ┃ ┃ ┗ 📄VaccineRepository.cs
 ┃ ┣ 📂UnitOfWork
 ┃ ┃ ┗ 📄UnitOfWork.cs
 ┃ ┣ 📄DependencyInjection.cs
 ┃ ┗ 📄FinlayDbContext.cs
```
