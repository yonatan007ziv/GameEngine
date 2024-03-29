﻿using GameEngine.Core.Components;
using GameEngine.Core.Components.Fonts;
using GameEngine.Core.Components.Input.Buttons;
using GameEngine.Core.Components.Input.Events;
using GameEngine.Core.SharedServices.Interfaces;
using GraphicsEngine.Components.Extensions;
using GraphicsEngine.Components.Interfaces.Buffers;
using GraphicsEngine.Components.RendererSpecific.OpenTK;
using GraphicsEngine.Components.Shared;
using GraphicsEngine.Components.Shared.Data;
using GraphicsEngine.Services.Implementations.Shared;
using GraphicsEngine.Services.Interfaces;
using Microsoft.Extensions.Logging;
using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace GraphicsEngine.Services.Implementations.OpenTK.Renderer;

internal class OpenTKRenderer : BaseOpenTKRenderer, IInternalGraphicsRenderer
{
	private readonly ILogger logger;
	private readonly IBufferFactory bufferFactory;
	private readonly IBufferDeletor bufferDeletor;

	public IFactory<string, string, MeshRenderer> MeshFactory { get; }

	public event Action? LoadEvent;

	public event Action<MouseEventData>? MouseEvent;
	public event Action<KeyboardEventData>? KeyboardEvent;
	public event Action<GamepadEventData>? GamepadEvent;

	private readonly MouseEventData mouseEventData = new MouseEventData();
	private readonly KeyboardEventData keyboardEventData = new KeyboardEventData();
	// NOT IMPLEMENTED: private readonly GamepadEventData gamepadEventData = new GamepadEventData();

	public OpenTKRenderer(ILogger logger, IFactory<string, ModelData> modelFactory, IFactory<string, Material> materialFactory, IBufferFactory bufferFactory, IBufferDeletor bufferDeletor)
	{
		this.logger = logger;
		this.bufferFactory = bufferFactory;
		this.bufferDeletor = bufferDeletor;
		MeshFactory = new MeshRendererFactory(logger, new OpenTKDrawingCall(), modelFactory, materialFactory);

		MouseDown += (mouseButtonArgs) =>
		{
			mouseEventData.Set(MouseEventType.MouseButton, Vector2.Zero, mouseButtonArgs.Button.Translate(), true);
			MouseEvent?.Invoke(mouseEventData);
		};
		MouseUp += (mouseButtonArgs) =>
		{
			mouseEventData.Set(MouseEventType.MouseButton, Vector2.Zero, mouseButtonArgs.Button.Translate(), false);
			MouseEvent?.Invoke(mouseEventData);
		};
		MouseMove += (mouseMoveArgs) =>
		{
			mouseEventData.Set(MouseEventType.MouseMove, mouseMoveArgs.Position.ToNumerics(), MouseButton.None, false);
			MouseEvent?.Invoke(mouseEventData);
		};

		KeyDown += (keyboardButtonArgs) =>
		{
			keyboardEventData.Set(keyboardButtonArgs.Key.Translate(), true);
			KeyboardEvent?.Invoke(keyboardEventData);
		};
		KeyUp += (keyboardButtonArgs) =>
		{
			keyboardEventData.Set(keyboardButtonArgs.Key.Translate(), false);
			KeyboardEvent?.Invoke(keyboardEventData);
		};
	}

	protected override void Load()
	{
		LoadEvent?.Invoke();
	}

	public void SetMouseLocked(bool locked)
		=> LockMouse(locked);

	public void SetMousePosition(Vector2 position)
	{
		MousePosition = position;
	}

	public void SetDepthTest(bool enable)
	{
		if (enable)
			GL.Enable(EnableCap.DepthTest);
		else
			GL.Disable(EnableCap.DepthTest);
	}

	public void SetViewport(ViewPort viewport)
	{
		GL.Viewport(
			(int)((viewport.x - viewport.width / 2) * WindowSize.X),
			(int)((viewport.y - viewport.height / 2) * WindowSize.Y),
			(int)(viewport.width * WindowSize.X),
			(int)(viewport.height * WindowSize.Y)
			);
	}

	protected override void ErrorCallback(string msg, DebugSeverity severity)
	{
		switch (severity)
		{
			case DebugSeverity.DebugSeverityLow:
				logger.LogInformation("GL-INFO: {errorMsg}", msg);
				break;
			case DebugSeverity.DebugSeverityMedium:
				logger.LogError("GL-ERROR: {errorMsg}", msg);
				break;
			case DebugSeverity.DebugSeverityHigh:
				logger.LogCritical("GL-CRITICAL: {errorMsg}", msg);
				break;
			case DebugSeverity.DebugSeverityNotification:
				logger.LogTrace("GL-TRACE: {errorMsg}", msg);
				break;
		}
	}

	public void DrawGlyf(CharacterGlyf glyph, Vector2 centeredPosition)
	{
		foreach (CharacterContour contour in glyph.CharacterContours)
		{
			if (contour.Clockwise)
				GL.FrontFace(FrontFaceDirection.Cw);
			else
				GL.FrontFace(FrontFaceDirection.Ccw);

			IVertexBuffer vertexBuffer = bufferFactory.GenerateVertexBuffer();
			IIndexBuffer indexBuffer = bufferFactory.GenerateIndexBuffer();

			Vector2[] points = contour.Points.ToArray();
			for (int i = 0; i < points.Length; i++)
				points[i] += centeredPosition;

			vertexBuffer.WriteData(points);

			AttributeLayout[] attributeLayouts =
			{
				new AttributeLayout(typeof(float), 2),
			};

			IVertexArray vertexArray = bufferFactory.GenerateVertexArray(vertexBuffer, indexBuffer, attributeLayouts);

			vertexArray.Bind();
			GL.DrawArrays(PrimitiveType.LineLoop, 0, contour.Points.Count);

			bufferDeletor.DeleteBuffer(vertexBuffer.Id);
			bufferDeletor.DeleteBuffer(indexBuffer.Id);
			bufferDeletor.DeleteVertexArrayBuffer(vertexArray.Id);
		}
	}
}