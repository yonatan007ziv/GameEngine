using GameEngine.Services.Interfaces.Parsers;

namespace GameEngine.Services.Implementations.Serializers;

internal class JsonSerializer : ISerializer
{
	public string Serialize<T>(T data)
	{
		return System.Text.Json.JsonSerializer.Serialize<T>(data);
	}

	public T Deserialize<T>(string data)
	{
		return System.Text.Json.JsonSerializer.Deserialize<T>(data);
	}
}