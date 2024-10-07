using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Application.Corporate.DTO;

namespace Rapido.Services.Customers.Application.Corporate.Queries;

public sealed record GetCorporateCustomer(Guid Id) : IQuery<CorporateCustomerDto>;