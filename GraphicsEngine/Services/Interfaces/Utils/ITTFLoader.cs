using GameEngine.Core.Components.TrueTypeFont;

namespace GraphicsEngine.Services.Interfaces.Utils;

internal interface ITTFLoader
{
	TrueTypeFont LoadFont(string fontName);
}