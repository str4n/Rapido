using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Wallets.Events;

public sealed record IndividualOwnerTransformedIntoCorporate(Guid OwnerId) : IEvent;