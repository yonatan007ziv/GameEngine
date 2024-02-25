using GameEngine.Core.Components.Font;

namespace GraphicsEngine.Services.Interfaces.Utils;

internal interface ITTFLoader
{
	Font LoadFont(string fontName);
}