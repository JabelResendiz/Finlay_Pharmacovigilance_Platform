using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;

public static class SeedReports
{
    private static string? token;
    private static readonly HttpClient client = new()
    {
        BaseAddress = new Uri(Config.ApiBaseUrl)
    };

    private static Guid[]? vaccineIds;
    private static Guid[]? symptomIds;
    private static Guid[]? vaccinationCenterIds;
    private static Guid[]? lotIds;

    public static async Task Run()
    {
        Console.WriteLine("📋 Starting Reports Seed...");

        await LoginAsync();
        await LoadCatalogDataAsync();
        await SeedPublicReportsAsync();

        Console.WriteLine("✅ Reports seed completed!");
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
    }

    // =========================
    // 📚 LOAD CATALOG DATA
    // =========================
    private static async Task LoadCatalogDataAsync()
    {
        Console.WriteLine("📚 Loading catalog data...");

        // Get active vaccines
        var vaccinesResponse = await client.GetAsync("/api/GetCatalog/vaccines/actives");
        if (vaccinesResponse.IsSuccessStatusCode)
        {
            var vaccinesJson = await vaccinesResponse.Content.ReadAsStringAsync();
            using var vaccinesDoc = JsonDocument.Parse(vaccinesJson);
            // var vaccines = vaccinesDoc.RootElement.GetProperty("items").EnumerateArray()
            //     .Select(v => Guid.Parse(v.GetProperty("id").GetString()!))
            //     .ToArray();

            var vaccines = vaccinesDoc.RootElement.EnumerateArray()
    .Select(v => Guid.Parse(v.GetProperty("id").GetString()!))
    .ToArray();

            vaccineIds = vaccines;
            Console.WriteLine($"✅ Loaded {vaccines.Length} vaccines");
        }

        // Get active symptoms
        var symptomsResponse = await client.GetAsync("/api/GetCatalog/symptoms/actives");
        if (symptomsResponse.IsSuccessStatusCode)
        {
            var symptomsJson = await symptomsResponse.Content.ReadAsStringAsync();
            using var symptomsDoc = JsonDocument.Parse(symptomsJson);
            // var symptoms = symptomsDoc.RootElement.GetProperty("items").EnumerateArray()
            //     .Select(s => Guid.Parse(s.GetProperty("id").GetString()!))
            //     .ToArray();
            var symptoms = symptomsDoc.RootElement.EnumerateArray()
               .Select(s => Guid.Parse(s.GetProperty("id").GetString()!))
               .ToArray();


            symptomIds = symptoms;
            Console.WriteLine($"✅ Loaded {symptoms.Length} symptoms");
        }
    }


    private static async Task LoadVaccinationCenterAsync(int provinceId, int municipalityId)
    {
        Console.WriteLine("📚 Loading vaccinationCenter data ...");

        // Get active vaccines
        var vaccinesResponse = await client.GetAsync($"/api/VaccinationCenter/getByMunicipality?municipalityId={municipalityId}&provinceId={provinceId}");

        if (vaccinesResponse.IsSuccessStatusCode)
        {
            var vaccinesJson = await vaccinesResponse.Content.ReadAsStringAsync();
            using var vaccinesDoc = JsonDocument.Parse(vaccinesJson);
            // var vaccines = vaccinesDoc.RootElement.GetProperty("items").EnumerateArray()
            //     .Select(v => Guid.Parse(v.GetProperty("id").GetString()!))
            //     .ToArray();

            var vaccines = vaccinesDoc.RootElement.EnumerateArray()
    .Select(v => Guid.Parse(v.GetProperty("id").GetString()!))
    .ToArray();

            vaccinationCenterIds = vaccines;
            Console.WriteLine($"✅ Loaded {vaccines.Length} vaccinationCenters");
        }


    }

    private static async Task LoadLotAsync(Guid vaccineId)
    {
        Console.WriteLine($"📚 Loading lot data from {vaccineId}...");

        // Get active vaccines
        var vaccinesResponse = await client.GetAsync($"/api/Lot/getByVaccine?vaccineId={vaccineId}");

        if (vaccinesResponse.IsSuccessStatusCode)
        {
            var vaccinesJson = await vaccinesResponse.Content.ReadAsStringAsync();
            using var vaccinesDoc = JsonDocument.Parse(vaccinesJson);


            var vaccines = vaccinesDoc.RootElement.EnumerateArray()
    .Select(v => Guid.Parse(v.GetProperty("id").GetString()!))
    .ToArray();

            lotIds = vaccines;
            Console.WriteLine($"✅ Loaded {vaccines.Length} lot");
        }

    }

    // =========================
    // 📋 PUBLIC REPORTS SEED
    // =========================
    private static async Task SeedPublicReportsAsync()
    {
        if (vaccineIds == null || vaccineIds.Length == 0 || symptomIds == null || symptomIds.Length == 0)
        {
            Console.WriteLine("❌ No catalog data available. Please seed vaccines and symptoms first.");
            return;
        }

        Console.WriteLine("📋 Seeding public reports...");

        var reports = await GenerateReports();

        for (int i = 0; i < reports.Length; i++)
        {
            var response = await client.PostAsJsonAsync("/api/Report/createPublic", reports[i]);
            Console.WriteLine($"➡ Report {i + 1}: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"   Error: {errorContent}");
            }

            // Small delay to avoid overwhelming the server
            await Task.Delay(100);
        }
    }

    private static int identityNumberCounter = 1000;
    private const string identityNumberBase = "030407";

    private static string GetNextIdentityNumber()
    {
        return $"{identityNumberBase}{identityNumberCounter++.ToString("D5")}";
    }

    private static async Task<object[]> GenerateReports()
    {
        var relationships = new[] { "Parent", "Other" };
        var reporterNames = new[]
        {
            "Alejandro Mora", "María Fernández", "Roberto Díaz", "Sandra López", "Laura González",
            "Pablo Herrera", "Sonia Pérez", "Ana Torres", "Marco Reyes", "Ricardo Sánchez",
            "Clara Vega", "Patricia Cruz", "Jorge Martínez", "Claudio Ruiz", "Pilar Castillo",
            "Gabriela Ramos", "Natalia Romero", "Verónica Martín", "Olga Prieto", "Natalia Cruz"
        };
        var patientNames = new[]
        {
            "Alejandro Mora", "Carlos Fernández", "María Díaz", "Miguel López", "Daniel González",
            "Natalia Herrera", "Esteban Pérez", "Lucía Torres", "Sofía Reyes", "Ana Sánchez",
            "Gabriela Vega", "Diego Cruz", "Valeria Martínez", "Verónica Ruiz", "Ernesto Castillo",
            "Sergio Ramos", "Javier Romero", "Verónica Martín", "Leo Prieto", "Natalia Cruz"
        };
        var genders = new[] { 1, 1, 2, 1, 1, 2, 1, 2, 2, 2, 2, 1, 2, 2, 1, 1, 1, 2, 1, 2 };
        var descriptions = new[]
        {
            "Mild fever and headache", "Fatigue and muscle pain", "Nausea and dizziness", "Joint pain and chills",
            "Redness and tenderness at injection site", "Sore throat and chills", "Swelling at injection site",
            "Sweating and mild dizziness", "Low-grade fever", "Mild nausea", "Headache and fatigue",
            "Mild chills", "Mild headache", "Mild dizziness", "Mild muscle ache", "Tiredness and chills",
            "Muscle soreness", "Slight fever", "Mild fatigue", "Temporary chills"
        };

        var reports = new List<object>();



        for (int i = 0; i < 20; i++)
        {
            var reportDate = DateTime.Parse("2026-04-11T21:38:54.456Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            var vaccinationDate = DateTime.Parse("2026-04-09T19:35:54.456Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            var adverseEventDate = DateTime.Parse("2026-04-10T19:35:54.456Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            var adverseEventFinishDate = DateTime.Parse("2026-04-11T19:35:54.456Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

            var vaccineId = vaccineIds![i % vaccineIds.Length];
            var symptomId = symptomIds![i % symptomIds.Length];

            await LoadLotAsync(vaccineId);
            await LoadVaccinationCenterAsync(1, 2);

            var lotId = lotIds![i % lotIds.Length];
            var vaccinationCenterId = vaccinationCenterIds![i % vaccinationCenterIds.Length];

            Console.WriteLine(vaccinationCenterId.ToString());

            var report = new
            {
                reportDate = reportDate,
                token = "aksjaksjkasj",
                reporter = new
                {
                    fullName = reporterNames[i],
                    reporterRelationship = relationships[new Random().Next(0, relationships.Length)],
                    identityNumber = GetNextIdentityNumber(),
                    dateOfBirth = DateTime.Parse("2003-04-07T19:35:54.456Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                    provinceId = 1,
                    municipalityId = 2,
                    phoneNumber = $"555100{i + 1:D2}",
                    email = $"{reporterNames[i].Replace(" ", ".").ToLower()}@example.com"
                },
                vaccinatedSubject = new
                {
                    fullName = patientNames[i],
                    identityNumber = GetNextIdentityNumber(),
                    dateOfBirth = DateTime.Parse("2003-04-07T19:35:54.456Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                    gender = genders[i],
                    isPregnant = false,
                    provinceId = 1,
                    municipalityId = 2,
                    healthArea = "Centro",
                    address = $"Calle {i + 1} #{(i + 1) * 100}",
                    phoneNumber = $"555200{i + 1:D2}",
                    email = $"{patientNames[i].Replace(" ", ".").ToLower()}@example.com",
                    currentMedications = i % 3 == 0 ? "Ibuprofen" : "None",
                    allergies = i % 4 == 0 ? "Pollen" : "None",
                    medicalHistory = i % 5 == 0 ? "Asthma" : "Healthy"
                },
                vaccinations = new[]
                {
                    new
                    {
                        vaccineId = vaccineId.ToString(),
                        lotId = lotId.ToString(),
                        site = i % 2 == 0 ? "leftarm" : "rightarm",
                        doseNumber = (i % 4) + 1,
                        administrationDate = vaccinationDate,
                        vaccinationCenterId = vaccinationCenterId.ToString()
                    }
                },
                adverseEvents = new[]
                {
                    new
                    {
                        startDate = adverseEventDate,
                        finishDate = adverseEventFinishDate,
                        description = descriptions[i],
                        visitedDoctor = i % 3 == 0,
                        wentToEmergencyRoom = i% 4 == 0,
                        permanentDisability = i%5 == 0,
                        isLifeThreatening = i%6 == 0,
                        resultedInDeath = false,
                        deathDate = (string?)null,
                        currentStatus = i % 2 == 0 ? "Recovered" : "Recovering",
                        intensity = i%3 == 0 ? "Mild" : (i%3==1) ? "Severe" : "Moderate",
                        severityLevel = (i%3==0 || i%4==0 || i%5==0 || i%6 == 0) ? "Serious" : "NonSerious",
                        symptomId = symptomId.ToString()
                    }
                }
            };

            reports.Add(report);
        }

        return reports.ToArray();
    }
}