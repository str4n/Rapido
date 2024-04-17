using Rapido.Framework.Common.Time;

namespace Rapido.Services.Customers.Core.Entities.Lockout;

internal sealed class TemporaryLockout : Lockout
{
    public DateTime EndDate { get; set; }
    
    public TemporaryLockout(Guid customerId, string reason, string description, DateTime startDate, DateTime endDate) : base(customerId, reason, description, startDate)
    {
        EndDate = endDate;
    }

    public bool IsActive(DateTime now) => now < EndDate;
}