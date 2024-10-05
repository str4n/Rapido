using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Application.Individual.Commands;

public sealed record CompleteIndividualCustomer(Guid CustomerId, string Name, string FullName, string Country, 
    string Province, string City, string Street, string Postalcode, string Nationality) : ICommand;