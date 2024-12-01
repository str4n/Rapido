using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Framework.Contracts.Wallets.Commands;

public sealed record CreateCorporateOwner(Guid CustomerId, string Name, string TaxId, string Nationality) : ICommand;