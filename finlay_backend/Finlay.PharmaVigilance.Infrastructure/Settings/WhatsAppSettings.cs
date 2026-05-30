namespace Finlay.PharmaVigilance.Infrastructure.Settings
{
    public class WhatsAppSettings
    {
        public const string SECTION_NAME = "WhatsApp";

        /// <summary>
        /// URL base de la API de OpenWA (localhost:2785/api por defecto)
        /// </summary>
        public string ApiBaseUrl { get; set; } = "http://localhost:2785/api";
        
        /// <summary>
        /// API Key para autenticación con OpenWA
        /// </summary>
        public string ApiKey { get; set; } = "dev-admin-key";
        
        /// <summary>
        /// ID de la sesión WhatsApp activa en OpenWA
        /// </summary>
        public string SessionId { get; set; } = "default-session";
        
        /// <summary>
        /// Timeout en segundos para las solicitudes HTTP
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;
    }
}
