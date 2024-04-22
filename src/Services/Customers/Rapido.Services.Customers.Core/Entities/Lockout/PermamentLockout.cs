namespace Rapido.Services.Customers.Core.Entities.Lockout;

internal sealed class PermanentLockout : Lockout
{
    public bool Active { get; set; } = true;
    
    public PermanentLockout(Guid customerId, string reason, string description, DateTime startDate) : base(customerId, reason, description, startDate)
    {
    }

    public bool IsActive() => Active;
}