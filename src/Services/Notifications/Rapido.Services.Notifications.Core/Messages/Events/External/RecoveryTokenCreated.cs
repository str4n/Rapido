using Rapido.Framework.Common.Abstractions.Events;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Events;

public sealed record RecoveryTokenCreated(Guid UserId, string Token) : IEvent;