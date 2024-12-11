using MassTransit;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Messages.Commands;
using Rapido.Messages.Events;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Corporate.Domain.Customer;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Core.Common.Messages.Commands.Handlers;

internal sealed class CreateCustomerConsumer(ICustomerRepository repository, IClock clock, 
    IMessageBroker messageBroker) : IConsumer<CreateCustomer>
{
    private const string Individual = "Individual";
    private const string Corporate = "Corporate";
    
    public async Task Consume(ConsumeContext<CreateCustomer> context)
    {
        var message = context.Message;

        Customer customer = message.AccountType switch
        {
            Individual => new IndividualCustomer(message.CustomerId, message.Email, clock.Now()),
            Corporate => new CorporateCustomer(message.CustomerId, message.Email, clock.Now()),
            _ => null
        };

        if (customer is null)
        {
            return;
        }

        await repository.AddAsync(customer);
        await messageBroker.PublishAsync(new CustomerCreated(customer.Id));
    }
}