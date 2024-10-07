using Rapido.Services.Customers.Domain.Individual.Customer;

namespace Rapido.Services.Customers.Domain.Individual.Repositories;

public interface IIndividualCustomerRepository
{
    Task<IEnumerable<IndividualCustomer>> GetAllAsync();
    Task<IndividualCustomer> GetAsync(Guid id, bool tracking = true);
    Task<IndividualCustomer> GetAsync(string email, bool tracking = true);
    Task AddAsync(IndividualCustomer customer);
    Task UpdateAsync(IndividualCustomer customer);
}