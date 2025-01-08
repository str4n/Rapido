using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Services.Customers.Core.Common.Clients;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;
using Rapido.Services.Customers.Core.Corporate.Domain.Customer;

namespace Rapido.Services.Customers.Core.Corporate.Commands.Handlers;

internal sealed class CreateCorporateCustomerHandler(
    ICustomerRepository repository,
    IUserApiClient apiClient,
    IClock clock)
    : ICommandHandler<CreateCorporateCustomer>
{
    public async Task HandleAsync(CreateCorporateCustomer command, CancellationToken cancellationToken = default)
    {
        var email = command.Email;

        if (await repository.AnyWithEmailAsync(email))
        {
            throw new CustomerAlreadyExistsException($"Customer with email: {email} already exists.");
        }

        var user = await apiClient.GetAsync(email);

        if (user is null)
        {
            throw new UserNotFoundException($"User with email: {email} was not found.");
        }

        var customerId = user.UserId;

        var customer = new CorporateCustomer(customerId, user.Email,clock.Now());

        await repository.AddAsync(customer);
    }
}