using BuildingBlocks.Application.Contracts;

namespace Modules.UserAccess.Application.Users.CreateUser;

public class CreateUserCommand(
    string email,
    string password,
    string? firstNameRu,
    string? lastNameRu,
    string? patronymicRu,
    string? firstNameEn,
    string? lastNameEn,
    string? patronymicEn) : CommandBase<Guid>
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;

    public string? FirstNameRu { get; set; } = firstNameRu;
    public string? LastNameRu { get; set; } = lastNameRu;
    public string? PatronymicRu { get; set; } = patronymicRu;

    public string? FirstNameEn { get; set; } = firstNameEn;
    public string? LastNameEn { get; set; } = lastNameEn;
    public string? PatronymicEn { get; set; } = patronymicEn;
}