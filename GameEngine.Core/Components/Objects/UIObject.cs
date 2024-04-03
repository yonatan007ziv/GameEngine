using System.Drawing;

namespace GameEngine.Core.Components.Objects;

public abstract class UIObject : GameObject
{
	public string Text { get => TextData.Text; set => TextData.Text = value; }
	public Color TextColor { get => TextData.TextColor; set => TextData.TextColor = value; }
	public string FontName { get => TextData.FontName; set => TextData.FontName = value; }
	public float FontSize { get => TextData.FontSize; set => TextData.FontSize = value; }

	public UIObject(UIObject? parent = null) : base(parent)
	{

	}
}