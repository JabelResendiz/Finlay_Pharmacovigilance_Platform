using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Manual;

public class ManufacturerResponseDto
{
    public required string Name { get; set; }
    public required Guid Id { get; set; }
    public required string Country { get; set; }
}



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
        //await SeedVaccinesAsync();

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
        token = doc.RootElement.GetProperty("accessToken").GetString();

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
            new { Name = "Dolor de cabeza intenso", Category = "Neurological", IsActive = true, Description = "Severe headache after vaccination" },
            new { Name = "Fiebre leve", Category = "General", IsActive = true, Description = "Mild fever within 24h" },
            new { Name = "Fatiga post vacuna", Category = "General", IsActive = true, Description = "Temporary fatigue after dose" },
            new { Name = "Dolor muscular", Category = "Muscular", IsActive = true, Description = "Muscle pain after injection" },
            new { Name = "Inflamación local", Category = "Local", IsActive = true, Description = "Swelling at injection site" }
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

        // 1. Vacunas base (una por centro inicial)
        var baseVaccines = new[]
        {
        new
        {
            name = "Soberana 02",
            type = "mRNA",
            code = "CUBA-001",
            isActive = true,
            description = "Cuban recombinant protein vaccine",
            approvalDate = "2020-04-03T09:18:28.095Z",
            targetPathology = "asdasda",
            manufacturer = new
            {
                name = "IFV",
                id= "",
                isNew = true,
                country = "Cuba"
            }
        },
        new
        {
            name = "Abdala",
            type = "mRNA",
            code = "CUBA-002",
            isActive = true,
            description = "Another Cuban COVID-19 vaccine",
            approvalDate = "2020-07-01T09:18:28.095Z",
            targetPathology = "asdasda",
            manufacturer = new
            {
                name = "CIGB",
                id= "",
                isNew = true,
                country = "Cuba"
            }
        },
        new
        {
            name = "Pfizer-BioNTech",
            type = "mRNA",
            code = "PFZ-001",
            isActive = true,
            description = "mRNA vaccine for COVID-19",
            approvalDate = "2020-12-01T09:18:28.095Z",
            targetPathology = "asdasda",
            manufacturer = new
            {
                name = "Pfizer",
                id= "",
                isNew = true,
                country = "USA"
            }
        }
    };

        foreach (var v in baseVaccines)
        {
            var response = await client.PostAsJsonAsync(
                "/api/Catalog/register/vaccine",
                v
            );

            Console.WriteLine($"➡ Vaccine {v.name}: {response.StatusCode}");
        }

        // 2. Obtener manufacturers desde API
        var manufacturers = await client
            .GetFromJsonAsync<List<ManufacturerResponseDto>>("/api/Manufacturer");

        if (manufacturers == null)
            return;

        // 3. Filtrar IFV
        var ifv = manufacturers.FirstOrDefault(m => m.Name == "IFV");

        if (ifv == null)
        {
            Console.WriteLine("❌ IFV manufacturer not found");
            return;
        }

        // 4. Vacunas adicionales basadas en IFV
        var finlayVaccines = new[]
        {
        new
        {
            name = "VA-MENGOC-BC",
            type = "mRNA",
            code = "FIN-002",
            isActive = true,
            description = "Finlay Institute vaccine A",
            approvalDate = "2021-01-01T00:00:00.000Z",
            targetPathology = "asdasda",
            manufacturer = new
            {
                name = ifv.Name,
                id = ifv.Id,
                isNew = false,
                country = ifv.Country
            }
        },
        new
        {
            name = "Finlay Vaccine B",
            type = "mRNA",
            code = "FIN-002",
            isActive = true,
            description = "Finlay Institute vaccine B",
            approvalDate = "2021-06-01T00:00:00.000Z",
            targetPathology = "asdasda",
            manufacturer = new
            {
                name = ifv.Name,
                id = ifv.Id,
                isNew = false,
                country = ifv.Country
            }
        }
    };

        // 5. Insertar vacunas IFV derivadas
        foreach (var v in finlayVaccines)
        {
            var response = await client.PostAsJsonAsync(
                "/api/Catalog/register/vaccine",
                v
            );

            Console.WriteLine($"➡ Finlay Vaccine {v.name}: {response.StatusCode}");
        }
    }
}