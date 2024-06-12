using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Core.Commands;

//A backup method if for some reason the event consumer wouldn't work.
public sealed record CreateCustomer(string Email, string CustomerType) : ICommand;