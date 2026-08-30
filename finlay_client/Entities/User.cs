
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manual;

public static class User
{
    public static async Task RegisterAdmin()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri($"{Config.ApiBaseUrl}/api/Authentication/register/admins");

        for (int i = 1; i <= 3; i++)
        {

            var user = new
            {
                UserName = $"admin_username_{i}",
                Email = $"admin_email_{i}@example.com",
                Password = "Password_123!",
                PhoneNumber = "12345678"
            };

            // Serializa el objeto a JSON
            var content = new StringContent(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(string.Empty, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully registered user");


            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} for user ID");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                break;
            }
        }
    }





}
