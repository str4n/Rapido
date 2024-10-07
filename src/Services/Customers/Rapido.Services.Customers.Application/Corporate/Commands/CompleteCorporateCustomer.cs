using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Application.Corporate.Commands;

public sealed record CompleteCorporateCustomer(Guid CustomerId, string Name, string TaxId, string Country, 
    string Province, string City, string Street, string Postalcode, string Nationality) : ICommand;