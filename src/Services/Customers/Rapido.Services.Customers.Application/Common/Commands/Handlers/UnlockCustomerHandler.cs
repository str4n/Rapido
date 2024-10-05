using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Application.Common.Exceptions;
using Rapido.Services.Customers.Domain.Common.Repositories;

namespace Rapido.Services.Customers.Application.Common.Commands.Handlers;

internal sealed class UnlockCustomerHandler : ICommandHandler<UnlockCustomer>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;

    public UnlockCustomerHandler(ICustomerRepository customerRepository, IClock clock, IMessageBroker messageBroker)
    {
        _customerRepository = customerRepository;
        _clock = clock;
        _messageBroker = messageBroker;
    }
    
    public async Task HandleAsync(UnlockCustomer command)
    {
        var customer = await _customerRepository.GetAsync(command.CustomerId);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }

        customer.Unlock(_clock.Now());

        await _customerRepository.UpdateAsync(customer);
        await _messageBroker.PublishAsync(new CustomerUnlocked(customer.Id));
    }
}