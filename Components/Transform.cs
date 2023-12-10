﻿
using OpenTK.Mathematics;

namespace OpenGLRenderer.Components;

internal class Transform
{
	public Vector3 Position { get; set; } = Vector3.Zero;
	public Vector3 Rotation { get; set; } = Vector3.Zero;
	public Vector3 Scale { get; set; } = Vector3.One;
}