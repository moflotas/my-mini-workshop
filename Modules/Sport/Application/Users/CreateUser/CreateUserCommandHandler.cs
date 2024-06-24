using BuildingBlocks.Application.Configuration.Commands;
using Modules.Sport.Domain.Users;

namespace Modules.Sport.Application.Users.CreateUser;

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