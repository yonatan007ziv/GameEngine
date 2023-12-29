using GraphicsRenderer.Components.Shared;

namespace GraphicsRenderer.Services.Interfaces.Utils.Managers;

internal interface ISceneManager
{
	public Scene CurrentScene { get; }
	void LoadScene(string path);
}