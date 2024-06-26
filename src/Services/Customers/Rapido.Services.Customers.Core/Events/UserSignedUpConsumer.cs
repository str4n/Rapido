﻿using MassTransit;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Users.Events;
using Rapido.Services.Customers.Core.Entities.Customer;
using Rapido.Services.Customers.Core.Exceptions;
using Rapido.Services.Customers.Core.Repositories;

namespace Rapido.Services.Customers.Core.Events;

internal sealed class UserSignedUpConsumer : IConsumer<UserSignedUp>
{
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

        if (!Enum.TryParse(message.AccountType, out CustomerType type))
        {
            throw new InvalidCustomerTypeException();
        }

        var customer = new Customer(message.UserId, message.Email, type, _clock.Now());

        await _customerRepository.AddAsync(customer);
    }
}