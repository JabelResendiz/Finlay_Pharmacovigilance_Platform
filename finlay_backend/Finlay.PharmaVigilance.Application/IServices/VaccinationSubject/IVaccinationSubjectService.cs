using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;


public interface IVaccinatedSubjectService
{
    Task<VaccinatedSubject> GetOrCreateAsync(
        VaccinatedSubjectDto dto);
}