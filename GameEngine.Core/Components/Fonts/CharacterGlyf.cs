namespace GameEngine.Core.Components.Fonts;

public class CharacterGlyf
{
	// Backing fields
	private int _resolution;
	private float _fontSize;

	public int Resolution { get => _resolution; set { if (_resolution != value) { _resolution = value; foreach (CharacterContour contour in CharacterContours) contour.Resolution = value; } } }
	public float FontSize { get => _fontSize; set { if (_fontSize != value) { _fontSize = value; foreach (CharacterContour contour in CharacterContours) contour.FontSize = value; } } }

	// Normalized screen-space width and height
	public float Width => _fontSize;
	public float Height => _fontSize;

	public IReadOnlyCollection<CharacterContour> CharacterContours { get; }

	public CharacterGlyf(CharacterContour[] characterContours)
	{
		CharacterContours = characterContours;
	}
}