namespace Rapido.Services.Customers.Core.Common.Domain.Lockout;

public sealed class TemporaryLockout : Lockout
{
    public DateTime EndDate { get; set; }
    
    public TemporaryLockout(Guid customerId, string reason, string description, DateTime startDate, DateTime endDate) : base(customerId, reason, description, startDate)
    {
        EndDate = endDate;
    }

    public bool IsActive(DateTime now) => now < EndDate;
}