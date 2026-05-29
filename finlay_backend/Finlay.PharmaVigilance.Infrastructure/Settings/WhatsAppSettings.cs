namespace Finlay.PharmaVigilance.Infrastructure.Settings
{
    public class WhatsAppSettings
    {
        public const string SECTION_NAME = "WhatsApp";

        public string ApiBaseUrl { get; set; } = "http://localhost:2785/api";
        public string ApiKey { get; set; } = "dev-admin-key";
        public int TimeoutSeconds { get; set; } = 30;
    }
}
