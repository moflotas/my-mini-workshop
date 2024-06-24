using BuildingBlocks.Application;
using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Domain;
using Modules.UserAccess.Domain.Users;

namespace Modules.UserAccess.Application.Users.EditUser;

public class EditUserCommandHandler(IUserRepository userRepository, IPasswordManager passwordManager) : ICommandHandler<EditUserCommand>
{
    public async Task Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            throw new InvalidCommandException([$"User with id {request.UserId} not found"]);
        }

        user.Email = request.Email ?? user.Email;
        user.Password = request.Password ?? user.Password;
        user.FirstNameRu = request.FirstNameRu ?? user.FirstNameRu;
        user.LastNameRu = request.LastNameRu ?? user.LastNameRu;
        user.PatronymicRu = request.PatronymicRu ?? user.PatronymicRu;
        user.FirstNameEn = request.FirstNameEn ?? user.FirstNameEn;
        user.LastNameEn = request.LastNameEn ?? user.LastNameEn;
        user.PatronymicEn = request.PatronymicEn ?? user.PatronymicEn;
    }
}