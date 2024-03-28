using GameEngine.Core.Components.Fonts.TrueTypeFont.Tables;
using System.Numerics;

namespace GameEngine.Core.Components.Fonts;

public class Font
{
    private int _resolution;
    private float _fontSize;
    public int Resolution { get => _resolution; set { if (_resolution != value) { _resolution = value; foreach (CharacterGlyf glyf in CharacterMaps.Values) glyf.Resolution = value; } } }
    public float FontSize { get => _fontSize; set { if (_fontSize != value) { _fontSize = value; foreach (CharacterGlyf glyf in CharacterMaps.Values) glyf.FontSize = value; } } }

    public string FontName { get; }
    public string FontFamily { get; }
    public string FontSubFamily { get; }
    public string Style { get; }
    public string Version { get; }

    public IReadOnlyDictionary<char, CharacterGlyf> CharacterMaps { get; }

    // TTF font constructor
    internal Font(TTFHead head, TTFName name, TTFLoca loca, TTFGlyf glyf, TTFCmap cmap, TTFHhea hhea, TTFVhea vhea, TTFHmtx hmtx, TTFVmtx vmtx, TTFMaxp maxp)
    {
        FontName = name.Name[4];
        FontFamily = name.Name[1];
        FontSubFamily = name.Name[2];
        Version = head.FontRevision.ToString();

        CharacterGlyf[] glyphs = new CharacterGlyf[maxp.NumGlyphs];

        // Glyphs
        for (int i = 0; i < maxp.NumGlyphs; i++)
        {
            // Compound glyph (not supported)
            if (glyf.Glyphs[i].NumberOfContours < 0)
                continue;

            float glyphWidth = Math.Abs(glyf.Glyphs[i].XMax - glyf.Glyphs[i].XMin);
            float glyphHeight = Math.Abs(glyf.Glyphs[i].YMax - glyf.Glyphs[i].YMin);

            // Contours
            CharacterContour[] characterContours = new CharacterContour[glyf.Glyphs[i].NumberOfContours];
            for (int j = 0; j < glyf.Glyphs[i].NumberOfContours; j++)
            {
                List<Vector2> contourPoints = new List<Vector2>();
                List<int> controlPointIndexes = new List<int>();

                // Iterate through all of the points in the current contour
                int startingK = j == 0 ? 0 : (glyf.Glyphs[i].EndPtsOfContours[j - 1] + 1);
                int k = startingK;
                for (; k < glyf.Glyphs[i].EndPtsOfContours[j] + 1; k++)
                {
                    // Not on curve 
                    if ((glyf.Glyphs[i].FlagsSimple[k] & (int)TTFGlyf.SIMPLE_FLAGS.ON_CURVE) == 0)
                        controlPointIndexes.Add(k - startingK);

                    int x = glyf.Glyphs[i].XCoordinates[k], y = glyf.Glyphs[i].YCoordinates[k];
                    contourPoints.Add(new Vector2(x, y));
                }

                characterContours[j] = new CharacterContour(contourPoints.ToArray(), controlPointIndexes.ToArray(), glyphWidth, glyphHeight, 0);
            }

            glyphs[i] = new CharacterGlyf(characterContours);
        }

        #region temp - replace with cmap mappings
        Dictionary<char, CharacterGlyf> tempDictionary = new Dictionary<char, CharacterGlyf>();

        char start = 'A';
        for (int i = 0; i < 26; i++)
            tempDictionary.Add(start++, glyphs[i + 36]);

        start = 'a';
        for (int i = 0; i < 26; i++)
            tempDictionary.Add(start++, glyphs[i + 68]);

        List<KeyValuePair<char, CharacterGlyf>> characterMaps = new List<KeyValuePair<char, CharacterGlyf>>(tempDictionary);
        #endregion

        CharacterMaps = new Dictionary<char, CharacterGlyf>(characterMaps);
        FontSize = 1;
    }
}