using OpenGLRenderer.OpenGL;

namespace OpenGLRenderer.Resources.Shaders.Managed;

internal class DefaultShader : ShaderProgram
{
    public DefaultShader()
        : base(new ShaderSource("DefVertex.glsl"), new ShaderSource("DefFragment.glsl"))
    {
        
    }
}