using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Core.Corporate.DTO;

namespace Rapido.Services.Customers.Core.Corporate.Queries;

public sealed record GetCorporateCustomer(Guid Id) : IQuery<CorporateCustomerDto>;