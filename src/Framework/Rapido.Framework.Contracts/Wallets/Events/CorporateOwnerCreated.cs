using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Wallets.Events;

public sealed record CorporateOwnerCreated(Guid OwnerId, string Nationality) : IEvent;