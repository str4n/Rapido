using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Customers.Events;

public sealed record CustomerLocked(Guid CustomerId, string Email, DateTime LockoutEnds) : IEvent;