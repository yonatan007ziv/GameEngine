namespace GraphicsRenderer.Services.Interfaces.Renderer;

public interface IRenderer
{
	void Run();
	public void LockMouse(bool lockMouse);
	public void TurnVSync(bool vsync);
}