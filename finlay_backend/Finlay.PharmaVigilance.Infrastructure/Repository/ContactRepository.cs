using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class ContactRepository : GenericRepository<Contact>, IContactRepository
{
    private readonly FinlayDbContext _context;

    public ContactRepository(FinlayDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Contact?> GetByEmailAsync(string email)
    {
        return await _context.Contacts.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<IEnumerable<Contact>> GetActiveContactsAsync()
    {
        return await _context.Contacts
            .Where(c => c.IsActive)
            .ToListAsync();
    }

    public IQueryable<Contact> GetActiveContacts()
    {
        return _context.Contacts.Where(c => c.IsActive);
    }
}
