using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Core.Individual.DTO;

namespace Rapido.Services.Customers.Core.Individual.Queries;

public sealed record GetIndividualCustomer(Guid Id) : IQuery<IndividualCustomerDto>;