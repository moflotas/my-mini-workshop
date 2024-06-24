namespace BuildingBlocks.Application.Configuration;

public class Secrets
{
    public JwtSecrets Jwt { get; set; } = default!;
    public DatabaseSecrets Database { get; set; } = default!;

    public class JwtSecrets
    {
        public string Issuer { get; set; } = default!;
        public string Key { get; set; } = default!;
        public int ExpiresInMinutes { get; set; }
        public int RefreshExpiresInMinutes { get; set; }
    }

    public class DatabaseSecrets
    {
        public string ConnectionString { get; set; } = default!;
    }
}