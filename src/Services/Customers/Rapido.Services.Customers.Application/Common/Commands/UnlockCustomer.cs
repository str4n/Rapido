using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Application.Common.Commands;

public sealed record UnlockCustomer(Guid CustomerId) : ICommand;