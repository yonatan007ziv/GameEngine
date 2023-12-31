namespace GraphicsRenderer.Components.Shared.Data;

internal struct AttributeLayout
{
	public AttributeLayout(Type type, int size)
	{
		Type = type;
		Size = size;
	}

	public Type Type;
	public int Size;
}