namespace Rapido.Services.Customers.Domain.Common.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer.Customer>> GetAllAsync();
    Task<Customer.Customer> GetAsync(Guid id);
    Task<bool> AnyAsync(string name);
    Task UpdateAsync(Customer.Customer customer);
}