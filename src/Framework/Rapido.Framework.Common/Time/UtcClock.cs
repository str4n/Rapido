namespace Rapido.Framework.Common.Time;

public sealed class UtcClock : IClock
{
    public DateTime Now() => DateTime.UtcNow;
}