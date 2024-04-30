namespace Rapido.Framework.Observability.Logging;

public sealed class SerilogOptions
{
    public string Level { get; set; }
    public ConsoleOptions Console { get; set; } = new();
    public SeqOptions Seq { get; set; } = new();
}

public sealed class ConsoleOptions
{
    public bool Enabled { get; set; }
    public string Template { get; set; }
}

public sealed class SeqOptions
{
    public bool Enabled { get; set; }
    public string ConnectionString { get; set; }
}