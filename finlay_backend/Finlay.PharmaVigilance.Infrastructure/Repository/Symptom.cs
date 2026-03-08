

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class SymptomRepository : GenericRepository<Symptom>, ISymptomRepository
{
    public SymptomRepository(FinlayDbContext context) : base(context) { }
}