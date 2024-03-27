namespace GameEngine.Core.Components.Fonts;

public class CharacterGlyf
{
	private int _resolution;
	public int Resolution { get => _resolution; set { _resolution = value; foreach (CharacterContour contour in CharacterContours) contour.Resolution = value; } }

	public int Width { get; }
	public int Height { get; }
	public IReadOnlyCollection<CharacterContour> CharacterContours { get; }

	public CharacterGlyf(CharacterContour[] characterContours, int width, int height)
	{
		CharacterContours = characterContours;
		Width = width;
		Height = height;
	}
}