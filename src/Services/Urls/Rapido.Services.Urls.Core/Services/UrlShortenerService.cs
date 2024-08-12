using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Time;
using Rapido.Services.Urls.Core.DTO;
using Rapido.Services.Urls.Core.EF;
using Rapido.Services.Urls.Core.Entities;
using Rapido.Services.Urls.Core.Requests;

namespace Rapido.Services.Urls.Core.Services;

internal sealed class UrlShortenerService : IUrlShortenerService
{
    private readonly UrlsDbContext _dbContext;
    private readonly IUrlAliasGenerator _aliasGenerator;
    private readonly IClock _clock;

    public UrlShortenerService(UrlsDbContext dbContext, IUrlAliasGenerator aliasGenerator, IClock clock)
    {
        _dbContext = dbContext;
        _aliasGenerator = aliasGenerator;
        _clock = clock;
    }
    
    public async Task<ShortenedUrlDto> ShortenUrl(ShortenUrl request)
    {
        string alias;
        
        do
        {
            alias = await _aliasGenerator.Generate();
        } while (await _dbContext.ShortenedUrls.AnyAsync(x => x.Alias == alias));

        var longUrl = request.Url;
        var shortUrl = $"{request.Scheme}://{request.Host}/link/{alias}";
        
        var shortenedUrl = new ShortenedUrl(longUrl, shortUrl, alias, _clock.Now().AddHours(24));

        await _dbContext.ShortenedUrls.AddAsync(shortenedUrl);
        await _dbContext.SaveChangesAsync();

        return new ShortenedUrlDto(shortenedUrl.ShortUrl, shortenedUrl.Alias, shortenedUrl.Expiry);
    }

    public async Task<string> GetRedirection(string alias)
        => (await _dbContext.ShortenedUrls.SingleOrDefaultAsync(x => x.Alias == alias)).LongUrl;
}