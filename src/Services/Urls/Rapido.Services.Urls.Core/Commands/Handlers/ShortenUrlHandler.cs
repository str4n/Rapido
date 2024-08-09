using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Services.Urls.Core.EF;
using Rapido.Services.Urls.Core.Entities;
using Rapido.Services.Urls.Core.Services;

namespace Rapido.Services.Urls.Core.Commands.Handlers;

internal sealed class ShortenUrlHandler : ICommandHandler<ShortenUrl>
{
    private readonly UrlsDbContext _dbContext;
    private readonly IUrlAliasGenerator _aliasGenerator;
    private readonly IClock _clock;

    public ShortenUrlHandler(UrlsDbContext dbContext, IUrlAliasGenerator aliasGenerator, IClock clock)
    {
        _dbContext = dbContext;
        _aliasGenerator = aliasGenerator;
        _clock = clock;
    }
    
    public async Task HandleAsync(ShortenUrl command)
    {
        string alias;
        
        do
        {
            alias = await _aliasGenerator.Generate();
        } while (await _dbContext.ShortenedUrls.AnyAsync(x => x.Alias == alias));

        var longUrl = command.Url;
        var shortUrl = $"{command.Scheme}://{command.Host}/link/{alias}";
        
        var shortenedUrl = new ShortenedUrl(longUrl, shortUrl, alias, _clock.Now().AddHours(24));

        await _dbContext.ShortenedUrls.AddAsync(shortenedUrl);
        await _dbContext.SaveChangesAsync();
    }
}