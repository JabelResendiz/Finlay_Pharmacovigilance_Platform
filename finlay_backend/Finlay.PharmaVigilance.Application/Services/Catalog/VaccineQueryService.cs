using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services;


public class VaccineQueryService : GenericQueryService<Vaccine, GetVaccineDto>,
                                         IVaccineQueryService
{
    public VaccineQueryService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {

    }

    public async Task<PagedResultDto<GetVaccineDto>> GetActivesVaccine(PagedRequestDto paged)
    {
        var query = _unitOfWork.GetRepository<Vaccine>()
                    .GetAllByItems(v => v.IsActive);

        var totalCount = await query.CountAsync();

        var items = await _unitOfWork.GetRepository<Vaccine>()
                        .GetAllPaged((paged.PageNumber - 1) * paged.PageSize, paged.PageSize)
                        .ToListAsync();

        return new PagedResultDto<GetVaccineDto>
        {
            Items = items?.Select(_mapper.Map<GetVaccineDto>) ?? Enumerable.Empty<GetVaccineDto>(),
            TotalCount = totalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalCount
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                        : null,
            PreviousPageUrl = paged.PageNumber > 1
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                        : null

        };
    }

}