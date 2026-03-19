using System.ComponentModel.DataAnnotations;

public class SymptomDto
{
    [Required]
    [StringLength(120, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(800, MinimumLength = 1)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string StandardCode { get; set; } = string.Empty;
}