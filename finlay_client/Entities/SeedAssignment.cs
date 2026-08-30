using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Manual;

public static class SeedAssignment
{
    private static string? token;
    private static readonly HttpClient client = new()
    {
        BaseAddress = new Uri(Config.ApiBaseUrl)
    };

    private static Guid[]? medicalReviewerIds;
    private static Guid[]? aefiReportsIds;

    public static async Task Run()
    {
        Console.WriteLine("📋 Starting Assignment Seed...");

        await LoginAsync();
        await LoadMedicalReviewerAsync();
        await LoadAefiReportsAsync();
        await SeedMedicalAssignmentsAsync();

        Console.WriteLine("✅ Assignment seed completed!");
    }

    // =========================
    // 🔐 LOGIN
    // =========================
    private static async Task LoginAsync()
    {
        Console.WriteLine("🔐 Logging in...");

        var login = new
        {
            Email = "resp2@example.com",
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
    }

    // =========================
    // 📚 LOAD CATALOG DATA
    // =========================
    private static async Task LoadMedicalReviewerAsync()
    {
        Console.WriteLine("🧑‍⚕️ Loading medical reviewers...");

        var response = await client.GetAsync(
            "/api/MedicalReviewer/summary"
        );

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("❌ Failed to load medical reviewers");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return;
        }

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;

        medicalReviewerIds = root
            .EnumerateArray()
            .Select(x => Guid.Parse(
                x.GetProperty("id").GetString()!
            ))
            .ToArray();

        Console.WriteLine($"✅ Loaded {medicalReviewerIds.Length} medical reviewers");
    }
    // =========================
    // 📋 PUBLIC REPORTS SEED
    // =========================
    private static async Task LoadAefiReportsAsync()
    {
        Console.WriteLine("📋 Loading AEFI reports...");

        var response = await client.GetAsync(
            "/api/Report/sectionResponsible/assigned?pageNumber=1&pageSize=100"
        );

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("❌ Failed to load AEFI reports");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return;
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;

        if (!root.TryGetProperty("items", out var items))
        {
            Console.WriteLine("❌ 'items' not found in AEFI response");
            return;
        }

        aefiReportsIds = items
            .EnumerateArray()
            .Select(x => Guid.Parse(x.GetProperty("id").GetString()!))
            .ToArray();

        Console.WriteLine($"✅ Loaded {aefiReportsIds.Length} AEFI reports");
    }





    private static async Task SeedMedicalAssignmentsAsync()
    {
        Console.WriteLine("📌 Creating medical review assignments...");

        if (medicalReviewerIds == null || medicalReviewerIds.Length == 0 ||
            aefiReportsIds == null || aefiReportsIds.Length == 0)
        {
            Console.WriteLine("❌ Missing data for assignments");
            return;
        }

        int assignmentsToCreate = 5;

        var random = new Random();

        for (int i = 0; i < assignmentsToCreate; i++)
        {
            var reviewerId = medicalReviewerIds[0];
            var reportId = aefiReportsIds[i];

            var payload = new
            {
                medicalReviewerId = reviewerId,
                aefiReportId = reportId,
                assignedAt = DateTime.Parse("2026-04-13T19:35:54.456Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
            };

            var response = await client.PostAsJsonAsync(
                "/api/MedicalReviewAssignment/create",
                payload
            );

            Console.WriteLine($"➡ Assignment {i + 1}: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"   ❌ Error: {error}");
            }

            await Task.Delay(100);
        }

        Console.WriteLine("✅ Assignments seed completed");
    }
}