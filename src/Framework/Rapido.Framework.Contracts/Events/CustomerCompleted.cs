using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Events;

public sealed record CustomerCompleted(Guid CustomerId, string Name, string FullName, string Nationality) : IEvent;