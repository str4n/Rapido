using Rapido.Services.Customers.Domain.Common.Customer;
using Rapido.Services.Customers.Domain.Common.Exceptions;

namespace Rapido.Services.Customers.Domain.Individual.Customer;

public sealed class IndividualCustomer : Common.Customer.Customer
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