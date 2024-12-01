using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Framework.Contracts.Customers.Commands;

public sealed record CreateCustomer(Guid CustomerId, string Email, string AccountType) : ICommand;