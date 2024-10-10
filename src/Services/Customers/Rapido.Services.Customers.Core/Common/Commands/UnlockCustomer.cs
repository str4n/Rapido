using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Core.Common.Commands;

public sealed record UnlockCustomer(Guid CustomerId) : ICommand;