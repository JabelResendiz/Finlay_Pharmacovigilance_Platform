

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class VaccineRepository : GenericRepository<Vaccine>, IVaccineRepository
{
    public VaccineRepository(FinlayDbContext context) : base(context) { }
}