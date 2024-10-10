using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Exceptions;

namespace Rapido.Services.Customers.Core.Corporate.Domain.Customer;

public sealed class CorporateCustomer : Common.Domain.Customer.Customer
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