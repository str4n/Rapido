using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Messages;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Commands.Handlers;

internal sealed class ChangeNationalityHandler(ICustomerRepository customerRepository, IMessageBroker messageBroker)
    : ICommandHandler<ChangeNationality>
{
    public async Task HandleAsync(ChangeNationality command, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetCustomerAsync(command.Id);

        if (customer is null)
        {
            throw new CustomerNotFoundException(command.Id);
        }

        var nationality = new Nationality(command.Nationality);
        
        customer.ChangeNationality(nationality);

        await customerRepository.UpdateAsync(customer);
        await messageBroker.PublishAsync(new CustomerNationalityChanged(customer.Id));
    }
}