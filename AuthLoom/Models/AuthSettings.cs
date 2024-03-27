namespace AuthLoom.Models
{
    public class AuthSettings
    {
        public string PathPrefix { get; set; } = string.Empty;
        public bool SuperAdminEnabled { get; set; }
        public List<Endpoint> Endpoints { get; set; } = new List<Endpoint>();
        public Jwt Jwt { get; set; } = new();
    }
}
