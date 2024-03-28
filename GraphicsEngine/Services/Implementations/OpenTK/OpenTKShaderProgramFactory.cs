using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.RendererSpecific.OpenTK;
using GraphicsEngine.Components.Shared;

namespace GraphicsEngine.Services.Implementations.OpenTK;

internal class OpenTKShaderProgramFactory : IFactory<ShaderSource, ShaderSource, IShaderProgram>
{
    public bool Create(ShaderSource vertexShader, ShaderSource fragmentShader, out IShaderProgram result)
    {
        result = new OpenTKShaderProgram(vertexShader, fragmentShader);
        return true;
    }
}