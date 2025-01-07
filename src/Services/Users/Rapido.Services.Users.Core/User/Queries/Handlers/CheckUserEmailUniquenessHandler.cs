using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Users.Core.User.Repositories;

namespace Rapido.Services.Users.Core.User.Queries.Handlers;

internal sealed class CheckUserEmailUniquenessHandler : IQueryHandler<CheckUserEmailUniqueness, bool>
{
    private readonly IUserRepository _repository;

    public CheckUserEmailUniquenessHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> HandleAsync(CheckUserEmailUniqueness query)
        => await _repository.AnyAsync(query.Email);
}