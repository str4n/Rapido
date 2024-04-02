namespace Rapido.Framework.Common.Time;

internal sealed class UtcClock : IClock
{
    public DateTime Now() => DateTime.UtcNow;
}