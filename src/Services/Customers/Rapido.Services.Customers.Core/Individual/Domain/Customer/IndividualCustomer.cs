using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Exceptions;

namespace Rapido.Services.Customers.Core.Individual.Domain.Customer;

public sealed class IndividualCustomer : Common.Domain.Customer.Customer
{
    public FullName FullName { get; private set; }
    
    public IndividualCustomer(Guid id, Email email, DateTime createdAt) : base(id, email, createdAt)
    {
    }
    
    public void Complete(Name name, FullName fullName, Address address, Nationality nationality,
        DateTime completedAt)
    {
        if (IsCompleted)
        {
            throw new CustomerCompletedException(Id);
        }

        Name = name;
        FullName = fullName;
        Address = address;
        Nationality = nationality;
        CompletedAt = completedAt;
        IsCompleted = true;
    }
}