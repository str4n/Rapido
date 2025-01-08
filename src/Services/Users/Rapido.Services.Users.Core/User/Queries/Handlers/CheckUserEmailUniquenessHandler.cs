using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Users.Core.User.Repositories;

namespace Rapido.Services.Users.Core.User.Queries.Handlers;

internal sealed class CheckUserEmailUniquenessHandler(IUserRepository repository)
    : IQueryHandler<CheckUserEmailUniqueness, bool>
{
    public async Task<bool> HandleAsync(CheckUserEmailUniqueness query, CancellationToken cancellationToken = default)
        => await repository.AnyAsync(query.Email);
}