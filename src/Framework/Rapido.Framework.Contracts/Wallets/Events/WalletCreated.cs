using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Wallets.Events;

public sealed record WalletCreated(Guid WalletId, Guid OwnerId, string Currency) : IEvent;