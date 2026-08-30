using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;


public interface IReporterService
{
    Task<Reporter> GetOrCreateAsync(
        ReporterDto dto);
}