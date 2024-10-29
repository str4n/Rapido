using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Users.Core.DTO;
using Rapido.Services.Users.Core.Exceptions;
using Rapido.Services.Users.Core.Repositories;

namespace Rapido.Services.Users.Core.Queries.Handlers;

internal sealed class GetUserByEmailHandler : IQueryHandler<GetUserByEmail, UserDto>
{
    private readonly IUserRepository _repository;

    public GetUserByEmailHandler(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<UserDto> HandleAsync(GetUserByEmail query)
    {
        var email = query.Email;
        var user = await _repository.GetAsync(email, false);

        if (user is null)
        {
            throw new UserNotFoundException($"User with email: {email} was not found.");
        }

        return new UserDto(user.Id, user.Email, user.Role.Name, user.Type.ToString());
    }
}