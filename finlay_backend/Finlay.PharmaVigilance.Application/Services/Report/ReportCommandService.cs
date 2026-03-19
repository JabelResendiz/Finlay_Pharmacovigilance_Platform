using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.Services;


public class ReportCommandService : IReportCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public ReportCommandService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }



    public async Task<ReportDto> CreateAsync(ReportDto dto)
    {
        var report = _mapper.Map<AefiReport>(dto);
        var vaccinatedSubject = _mapper.Map<VaccinatedSubject>(dto.VaccinatedSubject);
        var reporter = _mapper.Map<Reporter>(dto.Reporter);
        var vaccination = _mapper.Map<Vaccination>(dto.Vaccination);
        var vaccine = _mapper.Map<Vaccine>(dto.Vaccination.Vaccine);

        vaccination.Vaccine = vaccine;
        report.Vaccination = vaccination;
        report.VaccinatedSubject = vaccinatedSubject;
        report.Reporter = reporter;
        //report.PhysicianId = dto.PhysicianId;

        await _unitOfWork.GetRepository<AefiReport>().CreateAsync(report);
        await _unitOfWork.CompleteAsync();
        
        return dto;
    }

    public async Task<ReportDto> UpdateAsync(ReportDto dto)
    {
        await _unitOfWork.CompleteAsync();
        return dto;
    }

    public async Task DeleteAsync(int reportId)
    {
        await _unitOfWork.CompleteAsync();
    }
}