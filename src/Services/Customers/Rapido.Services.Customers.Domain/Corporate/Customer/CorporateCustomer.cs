using Rapido.Services.Customers.Domain.Common.Customer;
using Rapido.Services.Customers.Domain.Common.Exceptions;

namespace Rapido.Services.Customers.Domain.Corporate.Customer;

public sealed class CorporateCustomer : Common.Customer.Customer
{
    public TaxId TaxId { get; private set; }
    
    public CorporateCustomer(Guid id, Email email, DateTime createdAt) : base(id, email, createdAt)
    {
    }
    
    public void Complete(Name name, Address address, Nationality nationality, TaxId taxId,
        DateTime completedAt)
    {
        if (IsCompleted)
        {
            throw new CustomerCompletedException(Id);
        }

        Name = name;
        Address = address;
        Nationality = nationality;
        TaxId = taxId;
        CompletedAt = completedAt;
        IsCompleted = true;
    }
}