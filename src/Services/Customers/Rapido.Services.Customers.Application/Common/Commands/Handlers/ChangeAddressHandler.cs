using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Application.Common.Exceptions;
using Rapido.Services.Customers.Domain.Common.Customer;
using Rapido.Services.Customers.Domain.Common.Repositories;

namespace Rapido.Services.Customers.Application.Common.Commands.Handlers;

internal sealed class ChangeAddressHandler : ICommandHandler<ChangeAddress>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMessageBroker _messageBroker;

    public ChangeAddressHandler(ICustomerRepository customerRepository, IMessageBroker messageBroker)
    {
        _customerRepository = customerRepository;
        _messageBroker = messageBroker;
    }
    
    public async Task HandleAsync(ChangeAddress command)
    {
        var customer = await _customerRepository.GetAsync(command.Id);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.Id);
        }

        var address = new Address(command.Country, command.Province, command.City, command.Street, command.PostalCode);
        
        customer.ChangeAddress(address);

        await _customerRepository.UpdateAsync(customer);
        await _messageBroker.PublishAsync(new CustomerAddressChanged(customer.Id));
    }
}