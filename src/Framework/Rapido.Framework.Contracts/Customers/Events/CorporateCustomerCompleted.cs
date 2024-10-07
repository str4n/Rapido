using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Customers.Events;

public sealed record CorporateCustomerCompleted(Guid CustomerId, string Name, string TaxId, string Nationality) : IEvent;