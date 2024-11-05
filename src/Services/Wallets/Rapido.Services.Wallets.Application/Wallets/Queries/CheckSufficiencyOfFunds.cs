using Rapido.Framework.Common.Abstractions.Queries;

namespace Rapido.Services.Wallets.Application.Wallets.Queries;

// Check if wallet has enough funds to complete transaction
public sealed record CheckSufficiencyOfFunds(Guid OwnerId, double Amount, string Currency) : IQuery<bool>;