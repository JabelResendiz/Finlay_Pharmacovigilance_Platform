using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.Services;

public class ContactCommandService : IContactCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ContactCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ContactDto> CreateAsync(CreateContactDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var existingContact = await _unitOfWork.ContactRepository.GetByEmailAsync(dto.Email);
        if (existingContact != null)
            throw new ArgumentException("El correo electrónico ya existe en la base de datos");

        var contact = _mapper.Map<Contact>(dto);
        await _unitOfWork.ContactRepository.CreateAsync(contact);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<ContactDto>(contact);

    }

    public async Task<CreateContactDto> UpdateAsync(CreateContactDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Report DTO cannot be null.");

            // TODO: Implement update logic with proper validation
            await _unitOfWork.CompleteAsync();
            return dto;
        }
        catch (ArgumentNullException ex)
        {
            throw new InvalidOperationException($"Validation error: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while updating the report: {ex.Message}", ex);
        }
    }

    public async Task DeleteAsync(int contactId)
    {
        try
        {
            if (contactId <= 0)
                throw new ArgumentException("Report ID must be greater than zero.", nameof(contactId));

            var report = await _unitOfWork.ContactRepository.GetByIdAsync(contactId);
            if (report == null)
                throw new KeyNotFoundException($"Report with ID {contactId} does not exist.");

            await _unitOfWork.ContactRepository.DeleteByIdAsync(contactId);
            await _unitOfWork.CompleteAsync();
        }
        catch (ArgumentException ex)
        {
            throw new InvalidOperationException($"Validation error: {ex.Message}", ex);
        }
        catch (KeyNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while deleting the report: {ex.Message}", ex);
        }
    }

}