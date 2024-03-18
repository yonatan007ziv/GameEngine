namespace GameEngine.Core.Components.Fonts;

public class CharacterGlyf
{
	public CharacterContour[] CharacterContours { get; }

	public CharacterGlyf(CharacterContour[] characterContours)
	{
		CharacterContours = characterContours;
	}
}