using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Customers.Application.Individual.DTO;

namespace Rapido.Services.Customers.Application.Individual.Queries;

public sealed record GetIndividualCustomer(Guid Id) : IQuery<IndividualCustomerDto>;