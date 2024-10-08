using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Application.Common.Commands;

public sealed record ChangeAddress(Guid Id, string Country, string Province, string City, 
    string Street, string PostalCode) : ICommand;