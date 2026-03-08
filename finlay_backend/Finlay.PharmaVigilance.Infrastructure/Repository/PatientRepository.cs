

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(FinlayDbContext context) : base(context) { }
}