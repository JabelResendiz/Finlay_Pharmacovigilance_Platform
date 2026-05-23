using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Manual;

public static class SeedLot
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
        await SeedLotAsync();

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
            Email = "admin_email_2@example.com",
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
    // 💊 LOTS SEED
    // =========================
    private static async Task SeedLotAsync()
    {
        Console.WriteLine("🧠 Seeding lots...");

        // Obtener vacunas
        var vaccines = await client.GetFromJsonAsync<List<VaccineDto>>(
            "/api/GetCatalog/finlayVaccines"
        );

        if (vaccines == null || !vaccines.Any())
        {
            Console.WriteLine("❌ No vaccines found.");
            return;
        }

        var lots = new List<object>();

        foreach (var vaccine in vaccines)
        {
            // Tomar primeras 3 letras y convertir a mayúsculas
            var prefix = vaccine.Name.Length >= 3
                ? vaccine.Name.Substring(0, 3).ToUpper()
                : vaccine.Name.ToUpper();

            // Generar 5 lotes
            for (int i = 1; i <= 5; i++)
            {
                lots.Add(new
                {
                    LotNumber = $"{prefix}-00{i}",
                    VaccineId = vaccine.Id
                });
            }
        }

        // Registrar lotes
        foreach (var l in lots)
        {
            var response = await client.PostAsJsonAsync(
                "/api/Lot/register",
                l
            );

            Console.WriteLine(
                $"➡ Lot {((dynamic)l).LotNumber}: {response.StatusCode}"
            );
        }
    }

    public class VaccineDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
    }


}