using GameEngine.Components.Objects;
using GameEngine.Core.Components.Fonts;

namespace GameEngine.Components.UIComponents;

public class UILabel : UIObject
{
	public string Text { get => TextData.Text; set => TextData.Text = value; }
	public Font Font { get => TextData.Font; set => TextData.Font = value; }
	public int FontSize { get => TextData.FontSize; set => TextData.FontSize = value; }
}