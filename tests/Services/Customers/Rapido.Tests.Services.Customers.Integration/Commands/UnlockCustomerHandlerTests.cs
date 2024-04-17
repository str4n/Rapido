using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Services.Customers.Core.Commands;
using Rapido.Services.Customers.Core.Commands.Handler;
using Rapido.Services.Customers.Core.EF.Repositories;
using Rapido.Services.Customers.Core.Entities.Customer;
using Rapido.Services.Customers.Core.Repositories;
using Xunit;

namespace Rapido.Tests.Services.Customers.Integration.Commands;

public class UnlockCustomerHandlerTests : IDisposable
{
    private Task Act(UnlockCustomer command) => _handler.HandleAsync(command);
    
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }
    
    #region Arrange
    
    private readonly TestDatabase _testDatabase;
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;

    private UnlockCustomerHandler _handler;

    public UnlockCustomerHandlerTests()
    {
        _testDatabase = new TestDatabase();
        _customerRepository = new CustomerRepository(_testDatabase.DbContext);
        _clock = new TestClock();
        var messageBroker = new TestMessageBroker();

        _handler = new UnlockCustomerHandler(_customerRepository, _clock, messageBroker);
    }
    
    #endregion Arrange
}