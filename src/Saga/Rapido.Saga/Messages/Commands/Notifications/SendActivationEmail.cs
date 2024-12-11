using Rapido.Framework.Common.Abstractions.Commands;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Commands;

public sealed record SendActivationEmail(Guid UserId, string ActivationToken) : ICommand;