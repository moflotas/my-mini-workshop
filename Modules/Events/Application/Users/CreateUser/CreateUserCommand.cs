using BuildingBlocks.Application.Contracts;

namespace Modules.Events.Application.Users.CreateUser;

public class CreateUserCommand(
    Guid userId,
    string email,
    string? firstNameRu,
    string? lastNameRu,
    string? patronymicRu,
    string? firstNameEn,
    string? lastNameEn,
    string? patronymicEn) : CommandBase
{
    public Guid UserId { get; set; } = userId;
    public string Email { get; set; } = email;

    public string? FirstNameRu { get; set; } = firstNameRu;
    public string? LastNameRu { get; set; } = lastNameRu;
    public string? PatronymicRu { get; set; } = patronymicRu;

    public string? FirstNameEn { get; set; } = firstNameEn;
    public string? LastNameEn { get; set; } = lastNameEn;
    public string? PatronymicEn { get; set; } = patronymicEn;
}