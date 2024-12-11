using Rapido.Framework.Common.Abstractions.Events;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Events;

public sealed record IndividualCustomerCompleted(Guid CustomerId, string Name, string FullName, string Nationality) : IEvent;