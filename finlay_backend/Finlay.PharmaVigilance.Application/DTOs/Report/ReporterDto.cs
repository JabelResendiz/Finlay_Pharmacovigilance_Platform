using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReporterDto
{
    public required string FullName {get;set;}
    public required ReporterRelationship ReporterRelationship {get;set;}
    public required DateTime DateOfBirth {get;set;}
    public required int ProvinceId {get;set;}
    public required int MunicipalityId {get;set;}
    public required string? PhoneNumber {get;set;}
    public required string? Email {get;set;}
}