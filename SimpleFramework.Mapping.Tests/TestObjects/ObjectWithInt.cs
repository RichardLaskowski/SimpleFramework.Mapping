namespace SimpleFramework.Mapping.Tests;

public class ObjectWithInt
{
	public int Value { get; set; }

	public ObjectWithInt()
	{

	}

	public ObjectWithInt(int value)
	{
		Value = value;
	}
}

public class ObjectWithUInt
{
	public uint Value { get; set; }

	public ObjectWithUInt(uint value)
	{
		Value = value;
	}
}