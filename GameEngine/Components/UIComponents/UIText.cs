using GameEngine.Components.Objects;

namespace GameEngine.Components.UIComponents;

public class UIText : UIObject
{
	public string Text { get => TextData.Txt; set => TextData.Txt = value; }
	public string FontName { get => TextData.FontName; set => TextData.FontName = value; }
	public int FontSize { get => TextData.FontSize; set => TextData.FontSize = value; }
}