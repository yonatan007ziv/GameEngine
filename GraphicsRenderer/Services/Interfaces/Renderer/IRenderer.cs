namespace GraphicsRenderer.Services.Interfaces.Renderer;

public interface IRenderer
{
	string Title { get; set; }
	void Run();
	public void LockMouse(bool lockMouse);
	public void TurnVSync(bool vsync);
}