using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Wallets.Application.Wallets.Commands;

public sealed record CreateWallet(Guid OwnerId, string Currency) : ICommand;