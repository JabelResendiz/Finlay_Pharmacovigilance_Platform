using System.Text;

public static class SeedData
{
    public static List<SectionResponsibleDto> GenerateSectionResponsibles()
    {
        var list = new List<SectionResponsibleDto>();
        var provinces = GetProvinces();
        var municipalities = GetMunicipalities();
        int counter = 1;

        foreach (var p in provinces)
        {
            var munis = municipalities.Where(m => m.ProvinceId == p.Id);
            foreach (var m in munis)
            {
                list.Add(new SectionResponsibleDto
                {
                    UserName = $"responsable_{counter}_{NormalizeNameForUserName(p.Name)}",
                    Email = $"resp{counter}@example.com",
                    Password = "Password_123!",
                    PhoneNumber = $"55{counter:0000}",
                    ProvinceId = p.Id,
                    MunicipalityId = m.Id
                });
                counter++;
            }
        }

        return list;
    }

    public static string GetMunicipalityName(int municipalityId)
    {
        return GetMunicipalities()
            .FirstOrDefault(m => m.Id == municipalityId)?.Name
            ?? string.Empty;
    }

    public static List<Province> GetProvinces() => new()
    {
        new Province(1, "Pinar del Rio"),
        new Province(2, "Artemisa"),
        new Province(3, "Mayabeque"),
        new Province(4, "Isla de la Juventud"),
        new Province(5, "La Habana"),
        new Province(6, "Matanzas"),
        new Province(7, "Cienfuegos"),
        new Province(8, "Villa Clara"),
        new Province(9, "Sancti Spiritus"),
        new Province(10, "Ciego de Avila"),
        new Province(11, "Camaguey"),
        new Province(12, "Las Tunas"),
        new Province(13, "Granma"),
        new Province(14, "Holguin"),
        new Province(15, "Santiago de Cuba"),
        new Province(16, "Guantanamo")
    };

    public static List<Municipality> GetMunicipalities() => new()
    {
        new Municipality(1, "Pinar del Río", 1),
        new Municipality(2, "Viñales", 1),
        new Municipality(3, "Artemisa", 2),
        new Municipality(4, "Mariel", 2),
        new Municipality(5, "San José de las Lajas", 3),
        new Municipality(6, "Güines", 3),
        new Municipality(7, "Nueva Gerona", 4),
        new Municipality(8, "Isla de la Juventud rural", 4),
        new Municipality(9, "Plaza de la Revolución", 5),
        new Municipality(10, "Playa", 5),
        new Municipality(11, "Matanzas", 6),
        new Municipality(12, "Varadero", 6),
        new Municipality(13, "Cienfuegos", 7),
        new Municipality(14, "Cruces", 7),
        new Municipality(15, "Santa Clara", 8),
        new Municipality(16, "Caibarién", 8),
        new Municipality(17, "Sancti Spíritus", 9),
        new Municipality(18, "Trinidad", 9),
        new Municipality(19, "Ciego de Ávila", 10),
        new Municipality(20, "Morón", 10),
        new Municipality(21, "Camagüey", 11),
        new Municipality(22, "Florida", 11),
        new Municipality(23, "Las Tunas", 12),
        new Municipality(24, "Puerto Padre", 12),
        new Municipality(25, "Bayamo", 13),
        new Municipality(26, "Manzanillo", 13),
        new Municipality(27, "Holguín", 14),
        new Municipality(28, "Banes", 14),
        new Municipality(29, "Santiago de Cuba", 15),
        new Municipality(30, "Contramaestre", 15),
        new Municipality(31, "Guantánamo", 16),
        new Municipality(32, "Baracoa", 16)
    };

    public static string NormalizeNameForUserName(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var filtered = new StringBuilder();

        foreach (var c in normalized)
        {
            if (char.IsLetterOrDigit(c))
                filtered.Append(char.ToLowerInvariant(c));
        }

        return filtered.ToString();
    }
}

public record Province(int Id, string Name);
public record Municipality(int Id, string Name, int ProvinceId);
public record SectionResponsibleDto
{
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string PhoneNumber { get; init; }
    public required int ProvinceId { get; init; }
    public required int MunicipalityId { get; init; }
}