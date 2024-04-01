namespace Rapido.Framework.Postgres.Initializers;

public interface IDataInitializer
{
    Task InitAsync();
}