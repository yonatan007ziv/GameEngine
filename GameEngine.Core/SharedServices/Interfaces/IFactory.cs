namespace GameEngine.Core.SharedServices.Interfaces;

public interface IFactory<T>
{
	bool Create(out T result);
}

public interface IFactory<T1, T2>
{
	bool Create(T1 arg, out T2 result);
}
public interface IFactory<T1, T2, T3>
{
	bool Create(T1 arg1, T2 arg2, out T3 result);
}
public interface IFactory<T1, T2, T3, T4>
{
	bool Create(T1 arg1, T2 arg2, T3 arg3, out T4 result);
}