

using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class PhysicianRepository : GenericRepository<Physician>, IPhysicianRepository
{
    public PhysicianRepository(FinlayDbContext context) : base(context) { }
}