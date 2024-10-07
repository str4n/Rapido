using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Customers.Events;

public sealed record IndividualCustomerCompleted(Guid CustomerId, string Name, string FullName, string Nationality) : IEvent;