using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Core.DTO;

namespace Rapido.Services.Customers.Core.Queries;

public sealed record GetCustomers : IQuery<IEnumerable<CustomerDto>>;