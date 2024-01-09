namespace GameEngine.Core.Components;

public struct BoxData
{
	public RectangleData Front { get; set; }
	public RectangleData Back { get; set; }

	public BoxData(float right, float left, float top, float bottom, float front, float back)
	{
		Front = new RectangleData(right, left, top, bottom, front);
		Back = new RectangleData(right, left, top, bottom, back);
	}
}