namespace API.Modules.UserAccess.Users;

public class EditUserRequest
{
    public Guid UserId { get; set; } = default!;
    public string? Email { get; set; } = default!;
    public string? Password { get; set; } = default!;

    public string? FirstNameRu { get; set; } = default!;
    public string? LastNameRu { get; set; } = default!;
    public string? PatronymicRu { get; set; } = default!;

    public string? FirstNameEn { get; set; } = default!;
    public string? LastNameEn { get; set; } = default!;
    public string? PatronymicEn { get; set; } = default!;
}