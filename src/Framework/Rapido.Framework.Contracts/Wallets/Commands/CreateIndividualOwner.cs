using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Framework.Contracts.Wallets.Commands;

public sealed record CreateIndividualOwner(Guid CustomerId, string Name, string FullName, string Nationality) : ICommand;