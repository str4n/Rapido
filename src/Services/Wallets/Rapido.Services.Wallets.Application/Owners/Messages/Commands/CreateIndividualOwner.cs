using Rapido.Framework.Common.Abstractions.Commands;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Commands;

public sealed record CreateIndividualOwner(Guid CustomerId, string Name, string FullName, string Nationality) : ICommand;