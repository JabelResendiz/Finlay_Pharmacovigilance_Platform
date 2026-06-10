using System.Linq.Expressions;
using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IRepository;
using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Finlay.PharmaVigilance.Infrastructure.Repository;

public class ReportRepository : GenericRepository<AefiReport>, IReportRepository
{
    public ReportRepository(FinlayDbContext context) : base(context) { }

    public IQueryable<AefiReport> GetByFilter(
        string? vaccineName,
        string? provinceName,
        string? severity,
        string? reportStatus)
    {
        var query = _entity.AsQueryable();

        if (!string.IsNullOrWhiteSpace(vaccineName))
        {
            query = query.Where(ar => ar.Vaccinations.Any(v => v.Lot.Vaccine.Name == vaccineName));
        }

        if (!string.IsNullOrWhiteSpace(provinceName))
        {
            query = query.Where(ar => ar.VaccinatedSubject.Province.Name == provinceName);
        }

        if (!string.IsNullOrWhiteSpace(severity))
        {
            if (Enum.TryParse<SeverityLevel>(severity, true, out var severityEnum))
            {
                query = query.Where(r =>
                    r.AdverseEvents.Any(a =>
                        a.SeverityLevel == severityEnum));
            }

        }

        if (!string.IsNullOrWhiteSpace(reportStatus))
        {

            if (Enum.TryParse<ReportStatus>(reportStatus, true, out var statusEnum))
            {
                query = query.Where(r => r.Status == statusEnum);
            }
        }

        return query;

    }


    public IQueryable<AefiReport> GetSectionResponsibleByFilter(
        IQueryable<AefiReport> query,
        ReportSectionResponsibleFilter filter)
    {

        if (!string.IsNullOrWhiteSpace(filter.VaccineName))
        {
            query = query.Where(ar =>
            ar.Vaccinations.Any(v => v.Lot.Vaccine.Name == filter.VaccineName));
        }

        if (filter.VaccinationCenterId != null)
        {
            query = query.Where(ar => ar.Vaccinations.Any(v => v.VaccinationCenterId == filter.VaccinationCenterId));
        }

        if (!string.IsNullOrWhiteSpace(filter.Severity))
        {
            if (Enum.TryParse<SeverityLevel>(filter.Severity, true, out var severityEnum))
            {
                query = query.Where(r =>
                    r.AdverseEvents.Any(a =>
                        a.SeverityLevel == severityEnum));
            }

        }

        if (!string.IsNullOrWhiteSpace(filter.ReportStatus))
        {
            if (Enum.TryParse<ReportStatus>(filter.ReportStatus, true, out var statusEnum))
            {
                query = query.Where(r => r.Status == statusEnum);
            }

        }

        else
        {
            query = query.Where(r => r.Status == ReportStatus.Reopened || r.Status == ReportStatus.Submitted);
        }

        if (filter.From.HasValue)
        {
            query = query.Where(r => r.ReportDate >= filter.From.Value);
        }

        if (filter.To.HasValue)
        {
            query = query.Where(r => r.ReportDate <= filter.To.Value);
        }

        bool asc = filter.Order?.ToLower() == "asc";


        query = filter.SortBy?.ToLower() switch
        {
            "reportdate" => asc
                ? query.OrderBy(r => r.ReportDate)
                : query.OrderByDescending(r => r.ReportDate),

            "vaccinatedsubject.fullname" => asc
                ? query.OrderBy(r => r.VaccinatedSubject.FullName)
                : query.OrderByDescending(r => r.VaccinatedSubject.FullName),

            _ => query.OrderByDescending(r => r.ReportDate) // default
        };

        return query;

    }






    public IQueryable<AefiReport> GetMedicalReviewerByFilter(
        IQueryable<AefiReport> query,
        ReportMedicalReviewerFilter filter)
    {

        if (!string.IsNullOrWhiteSpace(filter.VaccineName))
        {
            query = query.Where(ar =>
            ar.Vaccinations.Any(v => v.Lot.Vaccine.Name == filter.VaccineName));
        }


        if (!string.IsNullOrWhiteSpace(filter.Severity))
        {
            if (Enum.TryParse<SeverityLevel>(filter.Severity, true, out var severityEnum))
            {
                query = query.Where(r =>
                    r.AdverseEvents.Any(a =>
                        a.SeverityLevel == severityEnum));
            }

        }

        bool asc = filter.Order?.ToLower() == "asc";

        query = filter.SortBy?.ToLower() switch
        {
            "reportdate" => asc
                ? query.OrderBy(r => r.ReportDate)
                : query.OrderByDescending(r => r.ReportDate),

            "vaccinatedsubject.fullname" => asc
                ? query.OrderBy(r => r.VaccinatedSubject.FullName)
                : query.OrderByDescending(r => r.VaccinatedSubject.FullName),

            _ => query.OrderByDescending(r => r.ReportDate) // default
        };

        return query;

    }


    public async Task<ReportStatusDto?> GetReportStatus(params Expression<Func<AefiReport, bool>>[] expressions)
    {

        var query = GetAllByItems(expressions);

        var data = await query
                    .GroupBy(r => 1)
                    .Select(g => new ReportStatusDto
                    {
                        TotalReports = g.Count(),
                        SubmittedReports = g.Count(x => x.Status == ReportStatus.Submitted),
                        UnderReviewReports = g.Count(x => x.Status == ReportStatus.UnderReview),
                        ApprovedReports = g.Count(x => x.Status == ReportStatus.Approved),
                        RejectedReports = g.Count(x => x.Status == ReportStatus.Rejected),
                        ReopenedReports = g.Count(x => x.Status == ReportStatus.Reopened),
                        ClosedReports = g.Count(x => x.Status == ReportStatus.Closed)
                    })
                    .FirstOrDefaultAsync();

        return data;
    }

    public async Task<IEnumerable<ProvinceReportStatusDto>> GetReportStatusByProvinces()
    {

        var data = await _entity
                         .GroupBy(r => r.VaccinatedSubject.Province.Name)
                         .Select(g => new ProvinceReportStatusDto
                         {
                             ProvinceName = g.Key,
                             Total = g.Count(),
                             Submitted = g.Count(x => x.Status == ReportStatus.Submitted),
                             UnderReview = g.Count(x => x.Status == ReportStatus.UnderReview),
                             Approved = g.Count(x => x.Status == ReportStatus.Approved),
                             Rejected = g.Count(x => x.Status == ReportStatus.Rejected),
                             Closed = g.Count(x => x.Status == ReportStatus.Closed),
                             Serious = g.Count(x => x.AdverseEvents
                                .OrderByDescending(a => a.SeverityLevel)
                                .Select(a => a.SeverityLevel)
                                .FirstOrDefault() == SeverityLevel.Serious)
                         })
                         .ToListAsync();

        return data;
    }



    public async Task<IEnumerable<CausalityDistributionDto>> GetCausalityDistributionAsync()
    {
        var total = await _context.MedicalReviews.CountAsync();

        if (total == 0)
            return Enumerable.Empty<CausalityDistributionDto>();

        return await _context.MedicalReviews
            .GroupBy(x => x.Causality)
            .Select(g => new CausalityDistributionDto
            {
                Causality = g.Key.ToString(),
                Count = g.Count(),
                Percentage = (double)g.Count() * 100 / total
            })
            .OrderByDescending(x => x.Count)
            .ToListAsync();
    }

    public async Task<IEnumerable<SignificanceDistributionDto>> GetSignificanceDistributionAsync()
    {
        var total = await _context.MedicalReviews.CountAsync();

        if (total == 0)
            return Enumerable.Empty<SignificanceDistributionDto>();

        return await _context.MedicalReviews
            .GroupBy(x => x.ClinicalSignificance)
            .Select(g => new SignificanceDistributionDto
            {
                Significance = g.Key.ToString(),
                Count = g.Count(),
                Percentage = (double)g.Count() * 100 / total
            })
            .OrderByDescending(x => x.Count)
            .ToListAsync();
    }



    public async Task<IEnumerable<SeverityLevelDistributionDto>> GetSeverityLevelDistributionAsync()
    {
        var totalReports = await _entity.CountAsync();

        if (totalReports == 0)
            return Enumerable.Empty<SeverityLevelDistributionDto>();

        var data = await _entity
            .GroupBy(x => 1)
            .Select(g => new
            {
                VisitedDoctor = g.Count(r =>
                    r.AdverseEvents.Any(a => a.VisitedDoctor)),

                WentToEmergencyRoom = g.Count(r =>
                    r.AdverseEvents.Any(a => a.WentToEmergencyRoom)),

                PermanentDisability = g.Count(r =>
                    r.AdverseEvents.Any(a => a.PermanentDisability)),

                Anomaly = g.Count(r =>
                    r.AdverseEvents.Any(a => a.Anomaly)),

                WasHospitalized = g.Count(r =>
                    r.AdverseEvents.Any(a => a.WasHospitalized)),

                ResultedInDeath = g.Count(r =>
                    r.AdverseEvents.Any(a => a.ResultedInDeath)),

                NoComplications = g.Count(r =>
                    r.AdverseEvents.Any(a => a.NoComplications))
            })
            .FirstOrDefaultAsync();

        if (data == null)
            return Enumerable.Empty<SeverityLevelDistributionDto>();

        return
        [
            new()
        {
            SeverityType = "Visited Doctor",
            Count = data.VisitedDoctor,
            Percentage = (double)data.VisitedDoctor * 100 / totalReports
        },
        new()
        {
            SeverityType = "Emergency Room",
            Count = data.WentToEmergencyRoom,
            Percentage = (double)data.WentToEmergencyRoom * 100 / totalReports
        },
        new()
        {
            SeverityType = "Permanent Disability",
            Count = data.PermanentDisability,
            Percentage = (double)data.PermanentDisability * 100 / totalReports
        },
        new()
        {
            SeverityType = "Congenital Anomaly",
            Count = data.Anomaly,
            Percentage = (double)data.Anomaly * 100 / totalReports
        },
        new()
        {
            SeverityType = "Hospitalized",
            Count = data.WasHospitalized,
            Percentage = (double)data.WasHospitalized * 100 / totalReports
        },
        new()
        {
            SeverityType = "Death",
            Count = data.ResultedInDeath,
            Percentage = (double)data.ResultedInDeath * 100 / totalReports
        },
        new()
        {
            SeverityType = "No Complications",
            Count = data.NoComplications,
            Percentage = (double)data.NoComplications * 100 / totalReports
        }
        ];
    }


    public async Task<IEnumerable<MonthlyReportTrendDto>> GetMonthlyReportTrendAsync()
    {
        var fromDate = DateTime.UtcNow.AddMonths(-11);

        return await _entity
            .Where(r => r.ReportDate >= fromDate)
            .GroupBy(r => new
            {
                r.ReportDate.Year,
                r.ReportDate.Month
            })
            .Select(g => new MonthlyReportTrendDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalReports = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();
    }

}