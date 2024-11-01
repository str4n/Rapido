using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Users.Core.Repositories;

namespace Rapido.Services.Users.Core.Queries.Handlers;

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