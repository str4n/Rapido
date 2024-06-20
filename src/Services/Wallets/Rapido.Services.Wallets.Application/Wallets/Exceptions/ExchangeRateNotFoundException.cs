using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Application.Wallets.Exceptions;

internal sealed class ExchangeRateNotFoundException : CustomException
{
    public ExchangeRateNotFoundException() : base("Exchange rate was not found.", ExceptionCategory.BadRequest)
    {
    }
}