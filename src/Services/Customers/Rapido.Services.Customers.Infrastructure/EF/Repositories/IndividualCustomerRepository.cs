using Microsoft.EntityFrameworkCore;
using Rapido.Services.Customers.Domain.Common.Customer;
using Rapido.Services.Customers.Domain.Individual.Customer;
using Rapido.Services.Customers.Domain.Individual.Repositories;

namespace Rapido.Services.Customers.Infrastructure.EF.Repositories;

internal sealed class IndividualCustomerRepository : IIndividualCustomerRepository
{
    private readonly CustomersDbContext _dbContext;

    public IndividualCustomerRepository(CustomersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<IndividualCustomer>> GetAllAsync()
        => await _dbContext.IndividualCustomers.ToListAsync();

    public Task<IndividualCustomer> GetAsync(Guid id, bool tracking)
        => tracking
            ? _dbContext.IndividualCustomers
                .Include(x => x.Lockouts)
                .SingleOrDefaultAsync(x => x.Id == id)
            : _dbContext.IndividualCustomers
                .AsNoTracking()
                .Include(x => x.Lockouts)
                .SingleOrDefaultAsync(x => x.Id == id);

    public Task<IndividualCustomer> GetAsync(string email, bool tracking)
        => tracking
            ? _dbContext.IndividualCustomers
                .Include(x => x.Lockouts)
                .SingleOrDefaultAsync(x => x.Email == email)
            : _dbContext.IndividualCustomers
                .AsNoTracking()
                .Include(x => x.Lockouts)
                .SingleOrDefaultAsync(x => x.Email == email);
    

    public async Task AddAsync(IndividualCustomer customer)
    {
        await _dbContext.IndividualCustomers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(IndividualCustomer customer)
    {
        _dbContext.IndividualCustomers.Update(customer);
        await _dbContext.SaveChangesAsync();
    }
}