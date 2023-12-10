namespace OpenGLRenderer.Services.Interfaces.Utils.Factories;

internal interface IFactory<T> where T : class
{
	T Create();
}