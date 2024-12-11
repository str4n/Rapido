using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Messages;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Commands.Handlers;

internal sealed class ChangeNationalityHandler : ICommandHandler<ChangeNationality>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMessageBroker _messageBroker;

    public ChangeNationalityHandler(ICustomerRepository customerRepository, IMessageBroker messageBroker)
    {
        _customerRepository = customerRepository;
        _messageBroker = messageBroker;
    }
    public async Task HandleAsync(ChangeNationality command)
    {
        var customer = await _customerRepository.GetCustomerAsync(command.Id);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.Id);
        }

        var nationality = new Nationality(command.Nationality);
        
        customer.ChangeNationality(nationality);

        await _customerRepository.UpdateAsync(customer);
        await _messageBroker.PublishAsync(new CustomerNationalityChanged(customer.Id));
    }
}