using GameEngine.Core.Components.Objects;

namespace GameEngine.Components.UIComponents;

public class UILabel : UIObject
{
	public string Text { get => TextData.Text; set => TextData.Text = value; }
	public string FontName { get => TextData.FontName; set => TextData.FontName = value; }
	public int FontSize { get => TextData.FontSize; set => TextData.FontSize = value; }
}