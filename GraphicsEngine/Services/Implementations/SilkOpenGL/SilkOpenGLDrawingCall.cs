using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Interfaces;
using Silk.NET.OpenGL;

namespace GraphicsEngine.Services.Implementations.SilkOpenGL;

internal class SilkOpenGLDrawingCall : IDrawingCall
{
	private readonly GL glContext;

	public SilkOpenGLDrawingCall()
	{
		glContext = SilkOpenGLContext.Instance.silkOpenGLContext;
	}

	public void DrawCall(ModelData modelData)
	{
		modelData.VertexArray.Bind();
		unsafe
		{
			glContext.DrawElements(PrimitiveType.Triangles, modelData.IndicesCount, DrawElementsType.UnsignedInt, null);
		}
		modelData.VertexArray.Unbind();
	}
}