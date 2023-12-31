using GraphicsRenderer.Components.Interfaces.Buffers;
using GraphicsRenderer.Components.Shared;
using GraphicsRenderer.Services.Interfaces.Utils;
using GraphicsRenderer.Services.Interfaces.Utils.Managers;

namespace GraphicsRenderer.Services.Implementations.Shared.Factories;

internal class GameObjectFactory : IFactory<GameObject>
{
	private readonly IShaderManager shaderManager;
	private readonly IBufferGenerator bufferGenerator;
	private readonly ITextureLoader textureLoader;
	private int currentId;

	public GameObjectFactory(IShaderManager shaderManager, IBufferGenerator bufferGenerator, ITextureLoader textureLoader)
	{
		this.shaderManager = shaderManager;
		this.bufferGenerator = bufferGenerator;
		this.textureLoader = textureLoader;
	}

	public GameObject Create()
	{
		ITextureBuffer tb = bufferGenerator.GenerateTextureBuffer();
		tb.WriteData(textureLoader.LoadTexture("MissingTexture.png"));
		return new GameObject(new Material(shaderManager.GetShader("Textured"), tb), currentId++);
	}
}