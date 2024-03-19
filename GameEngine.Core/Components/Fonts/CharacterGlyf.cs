using System.Collections.ObjectModel;

namespace GameEngine.Core.Components.Fonts;

public class CharacterGlyf
{
	public int Width { get; }
	public int Height { get; }
	public ReadOnlyCollection<CharacterContour> CharacterContours { get; }

	public CharacterGlyf(CharacterContour[] characterContours, int width, int height)
	{
		CharacterContours = characterContours.AsReadOnly();
		Width = width;
		Height = height;
	}
}