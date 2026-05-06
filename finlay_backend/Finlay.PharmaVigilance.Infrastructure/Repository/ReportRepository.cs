

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class ReportRepository : GenericRepository<AefiReport>, IReportRepository
{
    public ReportRepository(FinlayDbContext context) : base(context) { }

    public IQueryable<AefiReport> GetByFilter(string? vaccineName, string? provinceName)
    {
        var query = _entity.AsQueryable();

        if (!string.IsNullOrWhiteSpace(vaccineName))
        {
            query = query.Where(ar => ar.Vaccinations.Any(v => v.Vaccine.Name == vaccineName));
        }

        if (!string.IsNullOrWhiteSpace(provinceName))
        {
            query = query.Where(ar => ar.VaccinatedSubject.Province.Name == provinceName);
        }

        return query;

    }
}