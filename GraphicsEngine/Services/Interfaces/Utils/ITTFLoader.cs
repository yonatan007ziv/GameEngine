using GameEngine.Core.Components.Fonts;

namespace GraphicsEngine.Services.Interfaces.Utils;

internal interface ITTFLoader
{
	Font LoadFont(string fontName);
}