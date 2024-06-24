using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Domain;

namespace Modules.UserAccess.Domain.RefreshTokens;

public class RefreshToken : Entity, IAggregateRoot
{
    [Key] public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // For EF Core
    private RefreshToken()
    {
    }

    public static RefreshToken CreateRefreshToken(
        Guid userId,
        DateTime expiresAt)
    {
        return new RefreshToken(
            Guid.NewGuid(),
            userId,
            expiresAt,
            DateTime.UtcNow);
    }

    private RefreshToken(Guid id, Guid userId, DateTime expiresAt, DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        ExpiresAt = expiresAt;
        CreatedAt = createdAt;
    }
}