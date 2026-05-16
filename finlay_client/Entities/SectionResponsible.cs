using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

public static class SectionResponsibleSeed
{
    private static readonly HttpClient client = new()
    {
        BaseAddress = new Uri(Config.ApiBaseUrl)
    };

    private static readonly AdminCredentials AdminCredentials = new()
    {
        Email = "admin_email_1@example.com",
        Password = "Password_123!",
        UserName = "admin_username_1",
        PhoneNumber = "12345678"
    };

    public static async Task Run()
    {
        Console.WriteLine("🔥 Starting SectionResponsible seed...");

        //await EnsureAdminExistsAsync();
        await LoginAdminAsync();
        await SeedSectionResponsiblesAsync();

        Console.WriteLine("✅ SectionResponsible seed completed");
    }

    private static async Task EnsureAdminExistsAsync()
    {
        Console.WriteLine("🔧 Ensuring admin exists...");

        var adminPayload = new
        {
            UserName = AdminCredentials.UserName,
            Email = AdminCredentials.Email,
            Password = AdminCredentials.Password,
            PhoneNumber = AdminCredentials.PhoneNumber
        };

        var response = await client.PostAsJsonAsync("/api/Authentication/register/admins", adminPayload);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("✅ Admin created successfully.");
            return;
        }

        var content = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            Console.WriteLine("⚠️ Admin already exists. Continuing...");
            return;
        }

        Console.WriteLine($"⚠️ Admin ensure failed: {response.StatusCode}");
        Console.WriteLine(content);
    }

    private static async Task LoginAdminAsync()
    {
        Console.WriteLine("🔐 Logging in as admin...");

        var loginPayload = new
        {
            Email = AdminCredentials.Email,
            Password = AdminCredentials.Password
        };

        var response = await client.PostAsJsonAsync("/api/Authentication/login", loginPayload);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("❌ Admin login failed:");
            Console.WriteLine(json);
            throw new InvalidOperationException("No se pudo iniciar sesión con el admin de seed.");
        }

        using var doc = JsonDocument.Parse(json);
        var token = doc.RootElement.GetProperty("accessToken").GetString();
        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException("El token de admin no se recibió correctamente.");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        Console.WriteLine("✅ Admin login success.");
    }

    private static async Task SeedSectionResponsiblesAsync()
    {
        Console.WriteLine("🧠 Seeding SectionResponsibles...");

        foreach (var sr in SeedData.GenerateSectionResponsibles())
        {
            var response = await client.PostAsJsonAsync("/api/SectionResponsible/register", sr);
            var status = response.StatusCode;
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"{sr.UserName} -> {status}");
            if (!response.IsSuccessStatusCode)
                Console.WriteLine(body);
        }
    }

}


public record AdminCredentials
{
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string PhoneNumber { get; init; }
}