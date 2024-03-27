namespace GameEngine.Core.Components;

public class TextData
{
	public string Text { get; set; } = "";
	public string FontName { get; set; }
	public int FontSize { get; set; }

	public TextData()
	{
		FontName = "Arial.ttf";
		FontSize = 10;
	}
}