using Rapido.Framework.Common.Abstractions.Events;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Events;

public sealed record OwnerCreated(Guid OwnerId, string Nationality) : IEvent;