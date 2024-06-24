namespace API.Modules.UserAccess.Authentication;

public class RefreshRequest
{
    public string AccessToken { get; set; } = default!;
    public Guid RefreshToken { get; set; } = default!;
}