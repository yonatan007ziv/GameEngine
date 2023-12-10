﻿using OpenGLRenderer.Models;

namespace OpenGLRenderer.Services.Interfaces.Utils.Managers;

internal interface ISceneManager
{
	public Scene CurrentScene { get; }
	void LoadScene(string path);
}