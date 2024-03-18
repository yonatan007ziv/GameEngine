using GameEngine.Core.Components.Fonts;
using GameEngine.Core.SharedServices.Implementations.FontParsers;
using GameEngine.Core.SharedServices.Interfaces;

namespace GameEngine.Core.SharedServices.Implementations.FileReaders;

public class FontFileReader : IFileReader<Font>
{
	private readonly TrueTypeFontFileReader trueTypeFontFileReader;

	public FontFileReader(TrueTypeFontFileReader trueTypeFontFileReader)
    {
		this.trueTypeFontFileReader = trueTypeFontFileReader;
	}

    public bool ReadFile(string filePath, out Font result)
	{
		string extension = Path.GetExtension(filePath);

		if(extension == "ttf")
		{

		}

		throw new Exception("Unkown font file");
	}
}