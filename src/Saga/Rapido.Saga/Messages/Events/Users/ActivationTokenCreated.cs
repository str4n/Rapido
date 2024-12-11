using Rapido.Framework.Common.Abstractions.Events;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Events;

public sealed record ActivationTokenCreated(Guid UserId, string Token) : IEvent;