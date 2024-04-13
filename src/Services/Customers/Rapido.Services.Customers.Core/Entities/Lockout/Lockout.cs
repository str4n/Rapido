namespace Rapido.Services.Customers.Core.Entities.Lockout;

internal sealed class Lockout
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public string Reason { get; init; }
    public string Description { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; set; }

    public Lockout(Guid customerId, string reason, string description, DateTime startDate)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Reason = reason;
        Description = description;
        StartDate = startDate;
        EndDate = DateTime.MaxValue;
    }
    
    public Lockout(Guid customerId, string reason, string description, DateTime startDate, DateTime endDate)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        Reason = reason;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
    }
}