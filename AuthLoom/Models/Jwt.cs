namespace AuthLoom.Models
{
    public class Jwt
    {
        public string Secret { get; set; } = string.Empty;
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
    }
}
