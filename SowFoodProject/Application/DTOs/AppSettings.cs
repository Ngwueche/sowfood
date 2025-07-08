namespace SowFoodProject.Application.DTOs
{
    public class AppSettings
    {
        public int AmountToPay { get; set; }
        public string TermiiChannel { get; set; }
        public string TermiiFrom { get; set; }
        public string TermiiApiKey { get; set; }
        public int TermiiPinAttempts { get; set; }
        public int TermiiPinTimeToLive { get; set; }
        public int TermiiPinLength { get; set; }
        public string TermiiPinType { get; set; }
        public string TermiiBaseUrl { get; set; }
        public string TermiiMessageType { get; set; }
        public string TermiiGenerateEndpointUrl { get; set; }
        public string TermiiVerifyEndpointUrl { get; set; }
    }
}
