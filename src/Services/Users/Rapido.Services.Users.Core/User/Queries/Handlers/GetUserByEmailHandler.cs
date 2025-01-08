using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Users.Core.Shared.Exceptions;
using Rapido.Services.Users.Core.User.DTO;
using Rapido.Services.Users.Core.User.Repositories;

namespace Rapido.Services.Users.Core.User.Queries.Handlers;

internal sealed class GetUserByEmailHandler(IUserRepository repository) : IQueryHandler<GetUserByEmail, UserDto>
{
    public async Task<UserDto> HandleAsync(GetUserByEmail query, CancellationToken cancellationToken = default)
    {
        var email = query.Email;
        var user = await repository.GetAsync(email, false);

        if (user is null)
        {
            throw new UserNotFoundException($"User with email: {email} was not found.");
        }

        return new UserDto(user.Id, user.Email, user.Role.Name, user.Type.ToString());
    }
}