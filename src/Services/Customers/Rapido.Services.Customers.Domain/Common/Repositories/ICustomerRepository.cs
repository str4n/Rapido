namespace Rapido.Services.Customers.Domain.Common.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer.Customer>> GetAllAsync();
    Task<Customer.Customer> GetAsync(Guid id);
    Task UpdateAsync(Customer.Customer customer);
}