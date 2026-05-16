using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Manual;

public static class SeedVaccinationCenter
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
        await SeedVaccinationCenterAsync();

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
            Email = "admin_email_3@example.com",
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
    // 💊 VACCINATION CENTER SEED
    // =========================
    private static async Task SeedVaccinationCenterAsync()
    {
        Console.WriteLine("🧠 Seeding vaccinationCenter...");

        var vaccinationCenter = new List<object>();


        for (int i = 1; i <= 16; i++)
        {
            for (int j = 1; j <= 2; j++)
            {
                vaccinationCenter.Add(new
                {
                    Name = $"Policlinico_{i}_{j}",
                    Address = "anfjkdhsjfhsdfh",
                    MunicipalityId = (i - 1) * 2 + j,
                    ProvinceId = i
                });
            }

        }


        foreach (var v in vaccinationCenter)
        {
            var response = await client.PostAsJsonAsync(
                "/api/VaccinationCenter/register",
                v
            );

            Console.WriteLine($"➡ VaccinationCenter {((dynamic)v).Name}: {response.StatusCode}");
        }
    }


}