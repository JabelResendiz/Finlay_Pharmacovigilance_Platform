
using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface ICatalogCommandService
{
    Task<string> CreateVaccineAsync(VaccineDto vaccineDto);
    Task<string> CreateSymptomAsync(SymptomDto symptomDto);
}