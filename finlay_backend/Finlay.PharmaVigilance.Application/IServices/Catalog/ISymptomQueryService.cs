using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface ISymptomQueryService : IGenericQueryService<Symptom, GetSymptomDto>
{
    Task<PagedResultDto<GetSymptomDto>> GetActivesSymptoms(PagedRequestDto paged);

}