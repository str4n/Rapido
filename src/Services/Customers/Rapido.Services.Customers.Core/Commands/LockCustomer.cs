using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Core.Commands;

public sealed record LockCustomer(Guid CustomerId, string Reason, string Description, DateTime EndDate = default) : ICommand;