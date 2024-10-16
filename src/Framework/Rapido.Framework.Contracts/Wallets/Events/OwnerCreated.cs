using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Wallets.Events;

public sealed record OwnerCreated(Guid OwnerId, string Nationality) : IEvent;