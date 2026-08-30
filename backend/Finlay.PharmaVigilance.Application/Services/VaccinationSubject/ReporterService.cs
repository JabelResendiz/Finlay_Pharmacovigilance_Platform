using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Helper;

namespace Finlay.PharmaVigilance.Application.Services;

public class ReporterService
    : IReporterService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEncryptionService _crypto;

    public ReporterService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEncryptionService crypto)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _crypto = crypto ?? throw new ArgumentNullException(nameof(crypto));
    }

    public async Task<Reporter> GetOrCreateAsync(
        ReporterDto dto)
    {
        var blindIndex =
            _crypto.CreateBlindIndex(
                dto.IdentityNumber);

        var repository =
            _unitOfWork.GetRepository<Reporter>();

        var existing =
            await repository.FirstOrDefaultAsync(
                x => x.IdentityNumberBlindIndex
                     == blindIndex);

        if (existing != null)
        {
            return existing;
        }

        var entity =
            _mapper.Map<Reporter>(dto);

        entity.IdentityNumberEncrypted =
            _crypto.Encrypt(dto.IdentityNumber);

        entity.IdentityNumberBlindIndex =
            blindIndex;

        entity.DateOfBirth = IdentityNumberHelper.ExtractDateOfBirth(dto.IdentityNumber);

        return entity;
    }
}