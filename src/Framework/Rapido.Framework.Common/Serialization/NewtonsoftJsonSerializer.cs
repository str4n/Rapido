using Newtonsoft.Json;

namespace Rapido.Framework.Common.Serialization;

public sealed class NewtonsoftJsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerSettings _settings = new();
    
    public string Serialize<T>(T value)
        => JsonConvert.SerializeObject(value, _settings);

    public T Deserialize<T>(string value)
        => JsonConvert.DeserializeObject<T>(value, _settings);

    public object Deserialize(string value, Type type)
        => JsonConvert.DeserializeObject(value, type, _settings);
}