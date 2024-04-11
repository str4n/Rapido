using Rapido.Services.Customers.Core.Entities.Customer;

namespace Rapido.Services.Customers.Core.Repositories;

internal interface ICustomerRepository
{
    Task<Customer> GetAsync(Guid id, bool tracking = true);
    Task<Customer> GetAsync(string email, bool tracking = true);
    Task AddAsync(Customer customer);
}