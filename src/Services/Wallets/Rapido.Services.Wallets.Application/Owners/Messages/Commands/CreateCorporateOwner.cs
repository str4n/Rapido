using Rapido.Framework.Common.Abstractions.Commands;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Commands;

public sealed record CreateCorporateOwner(Guid CustomerId, string Name, string TaxId, string Nationality) : ICommand;