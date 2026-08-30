using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Globalization;

public static class MedicalReviewerSeed
{
    private static readonly HttpClient client = new()
    {
        BaseAddress = new Uri(Config.ApiBaseUrl)
    };

    private static readonly Random random = new();

    public static async Task Run()
    {
        Console.WriteLine("🔥 Starting MedicalReviewer seed...");

        var sectionResponsibles = SeedData.GenerateSectionResponsibles();
        foreach (var sr in sectionResponsibles)
        {
            Console.WriteLine($"\n🔐 Logging in as section responsible {sr.UserName} ({sr.Email})...");
            var token = await LoginSectionResponsibleAsync(sr);
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine($"⚠️ Skipping {sr.UserName} due to login error.");
                continue;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            await SeedMedicalReviewersForMunicipalityAsync(sr);
        }

        Console.WriteLine("✅ MedicalReviewer seed completed");
    }

    private static async Task<string?> LoginSectionResponsibleAsync(SectionResponsibleDto sr)
    {
        var loginPayload = new
        {
            Email = sr.Email,
            Password = sr.Password
        };

        var response = await client.PostAsJsonAsync("/api/Authentication/login", loginPayload);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"❌ Login failed for {sr.UserName}: {response.StatusCode}");
            Console.WriteLine(json);
            return null;
        }

        using var document = JsonDocument.Parse(json);
        return document.RootElement.GetProperty("accessToken").GetString();
    }

    private static int identityNumberCounter = 1000;
    private const string identityNumberBase = "030407";

    private static string GetNextIdentityNumber()
    {
        return $"{identityNumberBase}{identityNumberCounter++.ToString("D5")}";
    }
    private static async Task SeedMedicalReviewersForMunicipalityAsync(SectionResponsibleDto sr)
    {
        var municipalityName = SeedData.GetMunicipalityName(sr.MunicipalityId);
        var doctorCount = random.Next(3, 5);
        var normalizedMunicipality = SeedData.NormalizeNameForUserName(municipalityName);

        Console.WriteLine($"🩺 Creating {doctorCount} medical reviewers for {municipalityName}...");

        for (var index = 1; index <= doctorCount; index++)
        {
            var payload = new MedicalReviewerDto
            {
                UserName = $"medico_{normalizedMunicipality}_{index}",
                Email = $"medico_{normalizedMunicipality}_{sr.MunicipalityId}_{index}@example.com",
                Password = "Password_123!",
                PhoneNumber = $"55{sr.MunicipalityId:00}{index:00}{random.Next(10, 99)}",
                Institution = $"Centro de Salud {municipalityName}",
                ProfessionalLicense = $"MD-{random.Next(100000, 999999)}",
                IdentityNumber = GetNextIdentityNumber(),
                DateOfBirth = DateTime.Parse("2003-04-07T19:35:54.456Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                Specialty = GetRandomSpecialty()
            };

            // identityNumberBase = (long.Parse(identityNumberBase) + 1).ToString("00000");

            var response = await client.PostAsJsonAsync("/api/MedicalReviewer/register", payload);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"  ✅ {payload.UserName}");
            }
            else
            {
                Console.WriteLine($"  ❌ {payload.UserName}: {response.StatusCode}");
                Console.WriteLine(body);
            }
        }
    }

    private static string GetRandomSpecialty()
    {
        var specialties = new[]
        {
            "Medicina General",
            "Pediatría",
            "Ginecología",
            "Medicina Interna",
            "Traumatología",
            "Dermatología",
            "Oftalmología",
            "Psiquiatría",
            "Neurología",
            "Otorrinolaringología"
        };

        return specialties[random.Next(specialties.Length)];
    }
}

public record MedicalReviewerDto
{
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Institution { get; init; }
    public required string ProfessionalLicense { get; init; }
    public required string IdentityNumber { get; init; }
    public required DateTime? DateOfBirth { get; init; }
    public string? Specialty { get; init; }
}