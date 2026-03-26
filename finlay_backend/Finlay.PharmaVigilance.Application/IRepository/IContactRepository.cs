using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.IRepository;

public interface IContactRepository : IGenericRepository<Contact>
{
    Task<Contact?> GetByEmailAsync(string email);
    Task<IEnumerable<Contact>> GetActiveContactsAsync();

    IQueryable<Contact> GetActiveContacts();
}
