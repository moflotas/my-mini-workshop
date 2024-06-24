using BuildingBlocks.Application.Configuration.Commands;
using Modules.UserAccess.Domain.Users;

namespace Modules.UserAccess.Application.Users.CreateUser;

public class CreateUserCommandHandler(IUserRepository userRepository, IPasswordManager passwordManager) : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await User.CreateUser(
            Guid.NewGuid(),
            request.Email,
            passwordManager.HashPassword(request.Password),
            request.FirstNameRu,
            request.LastNameRu,
            request.PatronymicRu,
            request.FirstNameEn,
            request.LastNameEn,
            request.PatronymicEn,
            userRepository
        );

        await userRepository.AddAsync(user);

        return user.Id;
    }
}