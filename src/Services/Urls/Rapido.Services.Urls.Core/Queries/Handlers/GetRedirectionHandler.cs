using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Urls.Core.EF;

namespace Rapido.Services.Urls.Core.Queries.Handlers;

internal sealed class GetRedirectionHandler : IQueryHandler<GetRedirection, string>
{
    private readonly UrlsDbContext _dbContext;

    public GetRedirectionHandler(UrlsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> HandleAsync(GetRedirection query)
        => (await _dbContext.ShortenedUrls.SingleOrDefaultAsync(x => x.Alias == query.Alias)).LongUrl;
}