using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Core.Common.Commands;

public sealed record LockCustomerTemporarily(Guid CustomerId, string Reason, string Description, DateTime EndDate) : ICommand;