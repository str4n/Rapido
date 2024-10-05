using Rapido.Services.Customers.Domain.Corporate.Customer;

namespace Rapido.Services.Customers.Domain.Corporate.Repositories;

public interface ICorporateCustomerRepository
{
    Task<IEnumerable<CorporateCustomer>> GetAllAsync();
    Task<CorporateCustomer> GetAsync(Guid id, bool tracking = true);
    Task<CorporateCustomer> GetAsync(string email, bool tracking = true);
    Task<bool> AnyAsync(string name);
    Task AddAsync(CorporateCustomer customer);
    Task UpdateAsync(CorporateCustomer customer);
}