using System.Text.Json.Serialization;

namespace Finlay.PharmaVigilance.Application.DTO;


public class ActivateAccountTemplate : IBasicTemplate
{
    [JsonPropertyName("reviewer_name")]
    public string FullName { get; set; } = null!;

    [JsonPropertyName("activationLink")]
    public string ActivationLink { get; set; } = null!;
}