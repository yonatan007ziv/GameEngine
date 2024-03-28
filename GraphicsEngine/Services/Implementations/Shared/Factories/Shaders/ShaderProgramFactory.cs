using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.Shared;
using Microsoft.Extensions.Logging;

namespace GraphicsEngine.Services.Implementations.Shared.Factories.Shaders;

public class ShaderProgramFactory : IFactory<string, string, IShaderProgram>
{
    private readonly ILogger logger;
    private readonly IFactory<string, ShaderSource> shaderSourceFactory;
    private readonly IFactory<ShaderSource, ShaderSource, IShaderProgram> shaderProgramFactory;

    public ShaderProgramFactory(ILogger logger, IFactory<string, ShaderSource> shaderSourceFactory, IFactory<ShaderSource, ShaderSource, IShaderProgram> shaderProgramFactory)
    {
        this.logger = logger;
        this.shaderSourceFactory = shaderSourceFactory;
        this.shaderProgramFactory = shaderProgramFactory;
    }

    public bool Create(string vertexName, string fragmentName, out IShaderProgram shader)
    {
        shader = default!;

        if (!shaderSourceFactory.Create(vertexName, out ShaderSource vertexSource))
        {
            logger.LogError("GraphicsEngine. Error while creating a vertex shader source: {vertexShaderName}", vertexName);
            return false;
        }
        if (!shaderSourceFactory.Create(fragmentName, out ShaderSource fragmentSource))
        {
            logger.LogError("GraphicsEngine. Error while creating a fragment shader source: {fragmentShaderName}", fragmentName);
            return false;
        }

        if (!shaderProgramFactory.Create(vertexSource, fragmentSource, out shader))
        {
            logger.LogError("GraphicsEngine. Unknown error while creating a shader program");
            return false;
        }

        return true;
    }
}