using Rapido.Framework.Common.Abstractions.Events;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Events;

public sealed record CorporateCustomerCompleted(Guid CustomerId, string Name, string TaxId, string Nationality) : IEvent;