using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Core.Commands;

public sealed record CompleteCustomer(Guid CustomerId, string Name, string FullName, string Country, 
    string Province, string City, string Street, string Postalcode, string Nationality, string IdentityType, 
    string IdentitySeries) : ICommand;