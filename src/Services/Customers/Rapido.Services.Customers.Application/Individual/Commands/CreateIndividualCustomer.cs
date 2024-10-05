using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Application.Individual.Commands;

public sealed record CreateIndividualCustomer(string Email) : ICommand;