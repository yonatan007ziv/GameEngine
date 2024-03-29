using OpenTKVertexAttribPointerType = OpenTK.Graphics.OpenGL4.VertexAttribPointerType;
using SilKOpenGLVertexAttribPointerType = Silk.NET.OpenGL.VertexAttribPointerType;

namespace GraphicsEngine.Components.Extensions;

internal static class TypeExtensions
{
	public static OpenTKVertexAttribPointerType ToOpenTKType(this Type type)
	{
		if (type == typeof(sbyte))
			return OpenTKVertexAttribPointerType.Byte;
		else if (type == typeof(byte))
			return OpenTKVertexAttribPointerType.UnsignedByte;
		else if (type == typeof(short))
			return OpenTKVertexAttribPointerType.Short;
		else if (type == typeof(ushort))
			return OpenTKVertexAttribPointerType.UnsignedShort;
		else if (type == typeof(int))
			return OpenTKVertexAttribPointerType.Int;
		else if (type == typeof(uint))
			return OpenTKVertexAttribPointerType.UnsignedInt;
		else if (type == typeof(float))
			return OpenTKVertexAttribPointerType.Float;
		else if (type == typeof(double))
			return OpenTKVertexAttribPointerType.Double;
		else if (type == typeof(Half))
			return OpenTKVertexAttribPointerType.HalfFloat;
		throw new ArgumentException("Unsupported Type");
	}

	public static SilKOpenGLVertexAttribPointerType ToSilkOpenGLType(this Type type)
	{
		if (type == typeof(sbyte))
			return SilKOpenGLVertexAttribPointerType.Byte;
		else if (type == typeof(byte))
			return SilKOpenGLVertexAttribPointerType.UnsignedByte;
		else if (type == typeof(short))
			return SilKOpenGLVertexAttribPointerType.Short;
		else if (type == typeof(ushort))
			return SilKOpenGLVertexAttribPointerType.UnsignedShort;
		else if (type == typeof(int))
			return SilKOpenGLVertexAttribPointerType.Int;
		else if (type == typeof(uint))
			return SilKOpenGLVertexAttribPointerType.UnsignedInt;
		else if (type == typeof(float))
			return SilKOpenGLVertexAttribPointerType.Float;
		else if (type == typeof(double))
			return SilKOpenGLVertexAttribPointerType.Double;
		else if (type == typeof(Half))
			return SilKOpenGLVertexAttribPointerType.HalfFloat;
		throw new ArgumentException("Unsupported Type");
	}
}