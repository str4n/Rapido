using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Exceptions;
using Rapido.Services.Customers.Core.Repositories;

namespace Rapido.Services.Customers.Core.Commands.Handler;

internal sealed class VerifyCustomerHandler : ICommandHandler<VerifyCustomer>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;

    public VerifyCustomerHandler(ICustomerRepository customerRepository, IClock clock, IMessageBroker messageBroker)
    {
        _customerRepository = customerRepository;
        _clock = clock;
        _messageBroker = messageBroker;
    }
    
    public async Task HandleAsync(VerifyCustomer command)
    {
        var customer = await _customerRepository.GetAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }
        
        customer.Verify(_clock.Now());

        await _customerRepository.UpdateAsync(customer);
        await _messageBroker.PublishAsync(new CustomerVerified(customer.Id));
    }
}