using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Framework.Contracts.Wallets.Commands;

public sealed record CreateWallet(Guid OwnerId, string Nationality) : ICommand;