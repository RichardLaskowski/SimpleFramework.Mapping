namespace SimpleFramework.Mapping.Tests.TestObjects;
internal class ObjectWithValue<T>
{
	public T? Value { get; set; }

	public ObjectWithValue()
	{

	}
	public ObjectWithValue(T value)
	{
		Value = value;
	}
}
