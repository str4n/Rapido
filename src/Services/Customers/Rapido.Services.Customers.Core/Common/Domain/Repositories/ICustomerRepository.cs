using Rapido.Services.Customers.Core.Corporate.Domain.Customer;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Core.Common.Domain.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer.Customer>> GetCustomersAllAsync();
    Task<IndividualCustomer> GetIndividualCustomerAsync(Guid id);
    Task<CorporateCustomer> GetCorporateCustomerAsync(Guid id);
    Task<Customer.Customer> GetCustomerAsync(Guid id);
    Task<bool> AnyWithNameAsync(string name);
    Task<bool> AnyWithEmailAsync(string email);
    Task UpdateAsync(Customer.Customer customer);
    Task AddAsync(Customer.Customer customer);
}