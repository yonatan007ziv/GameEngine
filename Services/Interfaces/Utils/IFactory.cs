namespace OpenGLRenderer.Services.Interfaces.Utils;

internal interface IFactory<T> where T : class
{
	T Create();
}