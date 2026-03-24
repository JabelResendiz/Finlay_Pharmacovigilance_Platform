using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Application.Services;


public class ContactQueryService : GenericQueryService<Contact, ContactDto>,
                                  IContactQueryService
{
    public ContactQueryService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {

    }

    public async Task<IEnumerable<ContactDto>> GetActiveContactsAsync()
    {
        return await _unitOfWork.ContactRepository
                            .GetActiveContacts()
                            .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                            .ToListAsync();

        // return contacts.Select(_mapper.Map<ContactDto>);

    }
}