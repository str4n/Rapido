namespace Rapido.Services.Customers.Core.Common.Domain.Lockout;

public abstract class Lockout
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public string Reason { get; init; }
    public string Description { get; init; }
    public DateTime StartDate { get; init; }

    protected Lockout(Guid customerId, string reason, string description, DateTime startDate)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Reason = reason;
        Description = description;
        StartDate = startDate;
    }
}