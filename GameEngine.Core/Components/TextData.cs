namespace GameEngine.Core.Components;

public class TextData
{
    public string Text { get; set; } = "";
    public string FontName { get; set; }
    public float FontSize { get; set; }

    public TextData()
    {
        FontName = "Arial.ttf"; // Arial default font
        FontSize = 0.25f; // Typically ranges from 0 to 1
    }
}