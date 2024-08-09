namespace Rapido.Services.Urls.Core.Services;

internal interface IUrlAliasGenerator
{
    Task<string> Generate();
}