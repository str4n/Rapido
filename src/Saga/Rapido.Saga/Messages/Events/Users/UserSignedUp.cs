using Rapido.Framework.Common.Abstractions.Events;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Events;

public sealed record UserSignedUp(Guid UserId, string Email, string AccountType, DateTime CreatedAt) : IEvent;