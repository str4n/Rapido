using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Users.Core.Shared.Exceptions;
using Rapido.Services.Users.Core.User.DTO;
using Rapido.Services.Users.Core.User.Repositories;

namespace Rapido.Services.Users.Core.User.Queries.Handlers;

internal sealed class GetUserHandler(IUserRepository userRepository) : IQueryHandler<GetUser, UserDto>
{
    public async Task<UserDto> HandleAsync(GetUser query, CancellationToken cancellationToken = default)
    {
        var userId = query.UserId;
        var user = await userRepository.GetAsync(userId, false);

        if (user is null)
        {
            throw new UserNotFoundException($"User with id: {userId} was not found.");
        }

        return new UserDto(user.Id, user.Email, user.Role.Name, user.Type.ToString());
    }
}