using MassTransit;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Users.Events;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Corporate.Domain.Customer;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Core.Common.Events;

internal sealed class UserSignedUpConsumer : IConsumer<UserSignedUp>
{
    private const string Individual = "Individual";
    private const string Corporate = "Corporate";

    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;

    public UserSignedUpConsumer(ICustomerRepository customerRepository, IClock clock)
    {
        _customerRepository = customerRepository;
        _clock = clock;
    }
    
    public async Task Consume(ConsumeContext<UserSignedUp> context)
    {
        var message = context.Message;

        if (message.AccountType == Individual)
        {
            var customer = new IndividualCustomer(message.UserId, message.Email, _clock.Now());
            await _customerRepository.AddAsync(customer);
        }
        
        if (message.AccountType == Corporate)
        {
            var customer = new CorporateCustomer(message.UserId, message.Email, _clock.Now());
            await _customerRepository.AddAsync(customer);
        }
    }
}