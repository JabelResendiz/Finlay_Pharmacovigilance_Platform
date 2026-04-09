using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IVaccineQueryService : IGenericQueryService<Vaccine, GetVaccineDto>
{

}