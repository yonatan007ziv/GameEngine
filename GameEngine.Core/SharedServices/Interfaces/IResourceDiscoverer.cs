namespace GameEngine.Core.SharedServices.Interfaces;

public interface IResourceDiscoverer
{
	Dictionary<string, string> ResourceNamePathDictionary { get; }
}