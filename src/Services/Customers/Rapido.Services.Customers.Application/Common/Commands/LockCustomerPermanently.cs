using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Application.Common.Commands;

public sealed record LockCustomerPermanently(Guid CustomerId, string Reason, string Description) : ICommand;