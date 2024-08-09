namespace Rapido.Services.Urls.Core.Entities;

public sealed class ShortenedUrl
{
    public Guid Id { get; }
    public string LongUrl { get; private set; }
    public string ShortUrl { get; private set; }
    public string Alias { get; private set; }
    public DateTime Expiry { get; }

    public ShortenedUrl(Guid id, string longUrl, string shortUrl, string alias, DateTime expiry)
    {
        Id = id;
        LongUrl = longUrl;
        ShortUrl = shortUrl;
        Alias = alias;
        Expiry = expiry;
    }

    public ShortenedUrl(string longUrl, string shortUrl, string alias, DateTime expiry) 
        : this(Guid.NewGuid(), longUrl, shortUrl, alias, expiry)
    {
    }

    private ShortenedUrl()
    {
    }
}