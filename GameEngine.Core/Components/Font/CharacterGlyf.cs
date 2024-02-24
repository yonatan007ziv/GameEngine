namespace GameEngine.Core.Components.Font;

internal class CharacterGlyf
{
	public CharacterContour[] CharacterContours { get; }

    public CharacterGlyf(CharacterContour[] characterContours)
    {
        CharacterContours = characterContours;
    }
}