namespace GraphicsRenderer.Services.Interfaces.Utils;

internal interface IFactory<T>
{
	T Create();
}

internal interface IFactory<T1, T2>
{
	T2 Create(T1 arg);
}
internal interface IFactory<T1, T2, T3>
{
	T3 Create(T1 arg1, T2 arg2);
}