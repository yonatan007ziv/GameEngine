using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Interfaces;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Services.Interfaces.Utils.Managers;
namespace GraphicsEngine.Services.Implementations.Shared.Managers;

public class GLShaderManager : IShaderManager
{
    private readonly IFactory<string, string, IShaderProgram> shaderProgramFactory;
    private readonly Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();

    public GLShaderManager(IFactory<string, string, IShaderProgram> shaderProgramFactory)
    {
        this.shaderProgramFactory = shaderProgramFactory;
    }

    public bool GetShader(string shaderName, out Shader shader)
    {
        if (shaders.ContainsKey(shaderName))
        {
            shader = shaders[shaderName];
            return true;
        }
        else if (shaderProgramFactory.Create($"{shaderName}Vertex.glsl", $"{shaderName}Fragment.glsl", out IShaderProgram shaderProgram))
        {
            shader = new Shader(shaderProgram, shaderName);
            shaders.Add(shaderName, shader);
            return true;
        }

        shader = default!;
        return false;
    }
}