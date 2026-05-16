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
        token = doc.RootElement.GetProperty("token").GetString();

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

        var vaccines = new[]
        {
            "08deb05a-94b1-4ee5-874c-7ccf61f710dd",
            "08deb05a-aeb2-4757-8cd6-9cff873f0f11",
            "08deb05a-d1d8-4a10-8765-373a10823365",
            "08deb05a-dac3-4cab-8cf4-416e6aa4370a",
            "08deb05a-e2ee-4055-8053-19e7686eaa01"
        };

        var lotPrefixes = new[] { "SOB", "ABD", "PFZ", "MOD", "AZ" };
        var lots = new List<object>();

        var vaccines_with_prefixes = vaccines.Zip(lotPrefixes);

        foreach (var (vaccineId, prefix) in vaccines_with_prefixes)
        {
            for (int i = 1; i <= 5; i++)
            {
                lots.Add(new
                {
                    LotNumber = $"{prefix}-2024-{i:D3}",
                    VaccineId = vaccineId
                });
            }
        }

        foreach (var l in lots)
        {
            var response = await client.PostAsJsonAsync(
                "/api/Lot/register",
                l
            );

            Console.WriteLine($"➡ Lot {((dynamic)l).LotNumber}: {response.StatusCode}");
        }
    }


}