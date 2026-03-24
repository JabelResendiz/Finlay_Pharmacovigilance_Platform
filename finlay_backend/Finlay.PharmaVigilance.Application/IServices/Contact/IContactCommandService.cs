using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IContactCommandService : IGenericCommandService<CreateContactDto, ContactDto>
{

}