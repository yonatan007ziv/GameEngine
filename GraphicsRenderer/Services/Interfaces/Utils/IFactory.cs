namespace GraphicsRenderer.Services.Interfaces.Utils;

internal interface IFactory<T> where T : class
{
	T Create();
}

internal interface IFactory<T1, T2> where T2 : class
{
	T2 Create(T1 arg);
}
internal interface IFactory<T1, T2, T3> where T3 : class
{
	T3 Create(T1 arg1, T2 arg2);
}