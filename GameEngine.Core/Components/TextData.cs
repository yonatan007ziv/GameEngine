using System.Drawing;

namespace GameEngine.Core.Components;

public class TextData
{
	public string Text { get; set; } = "";
	public Color TextColor { get; set; } = Color.Black;
	public string FontName { get; set; }
	public float FontSize { get; set; }

	public TextData()
	{
		FontName = "Arial.ttf"; // Arial default font
		FontSize = 0.5f; // Typically ranges from 0 to 1
	}
}