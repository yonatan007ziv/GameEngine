using GameEngine.Core.Components.Fonts;

namespace GameEngine.Core.Components;

public class TextData
{
	public string Text { get; set; } = "";
	public Font Font { get; set; }
	public int FontSize { get; set; }

	public TextData(Font font, int fontSize = 12)
	{
		Font = font;
		FontSize = fontSize;
	}
}