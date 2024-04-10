using Rapido.Framework.Common.Time;

namespace Rapido.Tests.Services.Users.Integration;

public class TestClock : IClock
{
    public DateTime Now() => new(2024,04,08,10,0,0);
}