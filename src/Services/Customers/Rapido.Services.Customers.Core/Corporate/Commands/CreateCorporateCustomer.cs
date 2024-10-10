using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Core.Corporate.Commands;

public sealed record CreateCorporateCustomer(string Email) : ICommand;