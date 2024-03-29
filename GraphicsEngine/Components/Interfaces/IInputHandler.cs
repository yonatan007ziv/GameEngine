namespace GraphicsEngine.Components.Interfaces;

internal interface IInputHandler
{
	string Name { get; }
	void Disconnect();
}