using System.Globalization;

public static class Config
{
    private static readonly string[] EnvSearchPaths = new[]
    {
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env"),
        Path.Combine(Environment.CurrentDirectory, ".env")
    };

    public static string ApiBaseUrl { get; private set; } = "https://backend-production-3a9c6.up.railway.app"; // Default

    static Config()
    {
        foreach (var path in EnvSearchPaths)
        {
            if (!File.Exists(path))
                continue;

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("API_BASE_URL=", StringComparison.OrdinalIgnoreCase))
                {
                    ApiBaseUrl = trimmed.Substring("API_BASE_URL=".Length).Trim();
                    return;
                }
            }
        }
    }
}