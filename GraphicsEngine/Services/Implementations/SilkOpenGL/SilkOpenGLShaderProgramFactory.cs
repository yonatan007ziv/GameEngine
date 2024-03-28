using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.RendererSpecific.SilkOpenGL;
using GraphicsEngine.Components.Shared;

namespace GraphicsEngine.Services.Implementations.SilkOpenGL;

internal class SilkOpenGLShaderProgramFactory : IFactory<ShaderSource, ShaderSource, IShaderProgram>
{
    public bool Create(ShaderSource vertexShader, ShaderSource fragmentShader, out IShaderProgram result)
    {
        result = new SilkOpenGLShaderProgram(vertexShader, fragmentShader, SilkOpenGLContext.Instance.silkOpenGLContext);
        return true;
    }
}