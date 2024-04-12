using Rapido.Services.Customers.Core.Entities.Customer;

namespace Rapido.Services.Customers.Core.Repositories;

internal interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer> GetAsync(Guid id, bool tracking = true);
    Task<Customer> GetAsync(string email, bool tracking = true);
    Task<bool> AnyAsync(string name);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
}