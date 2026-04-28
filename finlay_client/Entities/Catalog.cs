using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Manual;

public static class SeedCatalog
{
    private static string? token;

    private static readonly HttpClient client = new()
    {
        BaseAddress = new Uri(Config.ApiBaseUrl)
    };

    public static async Task Run()
    {
        Console.WriteLine("🔥 Starting Seed...");

        await LoginAsync();
        await SeedSymptomsAsync();
        await SeedVaccinesAsync();

        Console.WriteLine("✅ Seed completed!");
    }

    // =========================
    // 🔐 LOGIN
    // =========================
    private static async Task LoginAsync()
    {
        Console.WriteLine("🔐 Logging in...");

        var login = new
        {
            Email = "admin_email_1@example.com",
            Password = "Password_123!"
        };

        var response = await client.PostAsJsonAsync("/api/Authentication/login", login);

        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("❌ Login failed:");
            Console.WriteLine(json);
            return;
        }

        using var doc = JsonDocument.Parse(json);
        token = doc.RootElement.GetProperty("token").GetString();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        Console.WriteLine("✅ Login success");

        Console.WriteLine($"Token: {token}");
    }

    // =========================
    // 💊 SYMPTOMS SEED
    // =========================
    private static async Task SeedSymptomsAsync()
    {
        Console.WriteLine("🧠 Seeding symptoms...");

        var symptoms = new[]
        {
            new { Name = "Dolor de cabeza intenso", StandardCode = "SYM001", CodingSystem = "ICD-10", Category = "Neurological", IsActive = true, Description = "Severe headache after vaccination" },
            new { Name = "Fiebre leve", StandardCode = "SYM002", CodingSystem = "ICD-10", Category = "General", IsActive = true, Description = "Mild fever within 24h" },
            new { Name = "Fatiga post vacuna", StandardCode = "SYM003", CodingSystem = "ICD-10", Category = "General", IsActive = true, Description = "Temporary fatigue after dose" },
            new { Name = "Dolor muscular", StandardCode = "SYM004", CodingSystem = "ICD-10", Category = "Muscular", IsActive = true, Description = "Muscle pain after injection" },
            new { Name = "Inflamación local", StandardCode = "SYM005", CodingSystem = "ICD-10", Category = "Local", IsActive = true, Description = "Swelling at injection site" }
        };

        foreach (var s in symptoms)
        {
            var response = await client.PostAsJsonAsync(
                "/api/Catalog/register/symptom",
                s
            );

            Console.WriteLine($"➡ Symptom {s.Name}: {response.StatusCode}");
        }
    }

    // =========================
    // 💉 VACCINES SEED
    // =========================
    private static async Task SeedVaccinesAsync()
    {
        Console.WriteLine("💉 Seeding vaccines...");

        var vaccines = new[]
        {
            new { Name = "Soberana 02", Type = 0, Code = "CUBA-001", IsActive = true, Description = "Cuban recombinant protein vaccine", ApprovalDate = "2020-04-03T09:18:28.095Z" },
            new { Name = "Abdala", Type = 0, Code = "CUBA-002", IsActive = true, Description = "Another Cuban COVID-19 vaccine", ApprovalDate = "2020-07-01T09:18:28.095Z" },
            new { Name = "Pfizer-BioNTech", Type = 0, Code = "PFZ-001", IsActive = true, Description = "mRNA vaccine for COVID-19", ApprovalDate = "2020-12-01T09:18:28.095Z" },
            new { Name = "Moderna", Type = 0, Code = "MOD-001", IsActive = true, Description = "mRNA vaccine developed in USA", ApprovalDate = "2020-11-15T09:18:28.095Z" },
            new { Name = "AstraZeneca", Type = 1, Code = "AZ-001", IsActive = true, Description = "Viral vector vaccine", ApprovalDate = "2020-08-01T09:18:28.095Z" }
        };

        foreach (var v in vaccines)
        {
            var response = await client.PostAsJsonAsync(
                "/api/Catalog/register/vaccine",
                v
            );

            Console.WriteLine($"➡ Vaccine {v.Name}: {response.StatusCode}");
        }
    }
}