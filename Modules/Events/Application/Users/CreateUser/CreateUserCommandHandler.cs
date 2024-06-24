using BuildingBlocks.Application.Configuration.Commands;
using Modules.Events.Domain.Users;

namespace Modules.Events.Application.Users.CreateUser;

public class CreateUserCommandHandler(IUserRepository userRepository) : ICommandHandler<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await User.CreateUser(
            request.UserId,
            request.Email,
            request.FirstNameRu,
            request.LastNameRu,
            request.PatronymicRu,
            request.FirstNameEn,
            request.LastNameEn,
            request.PatronymicEn
        );

        await userRepository.AddAsync(user);
    }
}