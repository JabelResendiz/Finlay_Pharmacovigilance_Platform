using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IContactQueryService : IGenericQueryService<Contact, ContactDto>
{
    Task<IEnumerable<ContactDto>> GetActiveContactsAsync();
}