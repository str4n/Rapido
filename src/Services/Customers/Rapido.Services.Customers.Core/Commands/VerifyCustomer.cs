using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Core.Commands;

public sealed record VerifyCustomer(Guid CustomerId) : ICommand;