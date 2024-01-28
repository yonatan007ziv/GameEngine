namespace GameEngine.Core.Components;

public struct ViewPort
{
	public float x, y, width, height;

	public ViewPort(float x, float y, float width, float height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}
}