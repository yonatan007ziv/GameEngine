using GameEngine.Core.Components.TrueTypeFont;
using GameEngine.Core.SharedServices.Interfaces;

namespace GameEngine.Core.SharedServices.Implementations.FileReaders;

internal class TrueTypeFontFileReader : IFileReader<TrueTypeFont>
{
    public bool ReadFile(string filePath, out TrueTypeFont result)
    {
        throw new NotImplementedException();
    }

    // Individual table parser
    // head
    // name
    // cmap
    // loca
    // glyf
    // maxp
    // hhea
    // vhea
    // hmtx
    // vmtx
}