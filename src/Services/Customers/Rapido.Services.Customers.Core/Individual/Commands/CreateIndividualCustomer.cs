using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Core.Individual.Commands;

public sealed record CreateIndividualCustomer(string Email) : ICommand;