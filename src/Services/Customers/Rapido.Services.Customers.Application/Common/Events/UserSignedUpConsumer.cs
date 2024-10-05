using MassTransit;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Users.Events;
using Rapido.Services.Customers.Domain.Common.Repositories;
using Rapido.Services.Customers.Domain.Corporate.Customer;
using Rapido.Services.Customers.Domain.Corporate.Repositories;
using Rapido.Services.Customers.Domain.Individual.Customer;
using Rapido.Services.Customers.Domain.Individual.Repositories;

namespace Rapido.Services.Customers.Application.Common.Events;

internal sealed class UserSignedUpConsumer : IConsumer<UserSignedUp>
{
    private const string Individual = "Individual";
    private const string Corporate = "Corporate";
    
    private readonly IIndividualCustomerRepository _individualCustomerRepository;
    private readonly ICorporateCustomerRepository _corporateCustomerRepository;
    private readonly IClock _clock;

    public UserSignedUpConsumer(IIndividualCustomerRepository individualCustomerRepository, 
        ICorporateCustomerRepository corporateCustomerRepository, IClock clock)
    {
        _individualCustomerRepository = individualCustomerRepository;
        _corporateCustomerRepository = corporateCustomerRepository;
        _clock = clock;
    }
    
    public async Task Consume(ConsumeContext<UserSignedUp> context)
    {
        var message = context.Message;

        if (message.AccountType == Individual)
        {
            var customer = new IndividualCustomer(message.UserId, message.Email, _clock.Now());
            await _individualCustomerRepository.AddAsync(customer);
        }
        
        if (message.AccountType == Corporate)
        {
            var customer = new CorporateCustomer(message.UserId, message.Email, _clock.Now());
            await _corporateCustomerRepository.AddAsync(customer);
        }
    }
}