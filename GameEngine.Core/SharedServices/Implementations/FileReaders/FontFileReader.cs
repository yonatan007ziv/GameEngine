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

    public bool ReadFile(string fontName, out Font result)
    {
        string extension = Path.GetExtension(fontName);

        if (extension == ".ttf")
            return trueTypeFontFileReader.ReadFile(fontName, out result);

        result = default!;
        return false;
    }
}