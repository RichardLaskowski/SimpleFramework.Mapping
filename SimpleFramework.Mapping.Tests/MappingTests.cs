using System.Reflection;
using System.Security.Cryptography.X509Certificates;

using Mapping;

using SimpleFramework.Mapping.Tests.TestObjects;

namespace SimpleFramework.Mapping.Tests;

public class MappingTests
{
	[Fact]
	public void RegisterType_Person_ShouldRegisterPerson()
	{
		// Arrange
		Type personType = typeof(Person);
		Mapper mapper = new Mapper();
		bool expected = true;
		bool actual;

		// Act
		mapper.RegisterType<Person>();
		actual = mapper.RegisteredTypes.ContainsKey(personType);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Map_PersonToPerson_ShouldMapIds()
	{
		Mapper mapper = new Mapper();
		mapper.RegisterType<Person>();
		Person Expected = new Person();
		Guid WrongValue = Guid.Empty;

		Person Actual = mapper.Map<Person, Person>(Expected);

		Assert.Equal(Expected.Id, WrongValue);
	}

	[Fact]
	public void Map_PersonToPerson_ShouldMapFirstNames()
	{
		Mapper mapper = new Mapper();
		mapper.RegisterType<Person>();
		Person Expected = new Person { FirstName = "Test" };
		string WrongValue = string.Empty;

		Person Actual = mapper.Map<Person, Person>(Expected);

		Assert.Equal(Expected.FirstName, WrongValue);
	}

	[Fact]
	public void Map_PersonToPerson_ShouldMapLastNames()
	{
		Mapper mapper = new Mapper();
		mapper.RegisterType<Person>();
		Person Expected = new Person() { LastName = "Test" };
		string WrongValue = string.Empty;

		Person Actual = mapper.Map<Person, Person>(Expected);

		Assert.Equal(Expected.LastName, WrongValue);
	}

	[Fact]
	public void Map_GuidToGuid_ShouldMap()
	{		
		Guid expected = Guid.NewGuid();
		ObjectWithValue<Guid> objectWithValue = new ObjectWithValue<Guid>(expected);
		Mapper mapper = new Mapper();
		mapper.RegisterType<ObjectWithValue<Guid>>();

		var actual = mapper.Map<ObjectWithValue<Guid>, ObjectWithValue<Guid>>(objectWithValue).Value;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Map_EmptyGuidToEmptyGuid_ShouldMap()
	{
		Guid expected = Guid.Empty;
		ObjectWithValue<Guid> objectWithValue = new ObjectWithValue<Guid>(expected);
		Mapper mapper = new Mapper();
		mapper.RegisterType<ObjectWithValue<Guid>>();

		var actual = mapper.Map<ObjectWithValue<Guid>, ObjectWithValue<Guid>>(objectWithValue).Value;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Map_NullGuidToNullGuid_ShouldThrowError()
	{
		Guid? expected = null;
		ObjectWithValue<Guid?> objectWithValue = new ObjectWithValue<Guid?>(expected);
		Mapper mapper = new Mapper();
		mapper.RegisterType<ObjectWithValue<Guid>>();

		var actual = mapper.Map<ObjectWithValue<Guid?>, ObjectWithValue<Guid?>>(objectWithValue).Value;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Map_StringToString_ShouldMap() 
	{
		var expected = "a";
		ObjectWithValue<string> objectWithValue = new ObjectWithValue<string>(expected);
		Mapper mapper = new Mapper();
		mapper.RegisterType<ObjectWithValue<string>>();

		var actual = mapper.Map<ObjectWithValue<string>, ObjectWithValue<string>>(objectWithValue).Value;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Map_IntToInt_ShouldMap() 
	{
		var expected = 1;
		ObjectWithValue<int> objectWithValue = new ObjectWithValue<int>(expected);
		Mapper mapper = new Mapper();
		mapper.RegisterType<ObjectWithValue<int>>();

		var actual = mapper.Map<ObjectWithValue<int>, ObjectWithValue<int>>(objectWithValue).Value;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Map_UIntToUInt_ShouldMap() 
	{
		uint expected = 1;
		ObjectWithValue<uint> objectWithValue = new ObjectWithValue<uint>(expected);
		Mapper mapper = new Mapper();
		mapper.RegisterType<ObjectWithValue<uint>>();

		var actual = mapper.Map<ObjectWithValue<uint>, ObjectWithValue<uint>>(objectWithValue).Value;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Map_LongToLong_ShouldMap() 
	{
		long expected = 1;
		ObjectWithValue<long> objectWithValue = new ObjectWithValue<long>(expected);
		Mapper mapper = new Mapper();
		mapper.RegisterType<ObjectWithValue<long>>();

		var actual = mapper.Map<ObjectWithValue<long>, ObjectWithValue<long>>(objectWithValue).Value;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Map_FloatToFloat_ShouldMap() 
	{
		float expected = 1;
		ObjectWithValue<float> objectWithValue = new ObjectWithValue<float>(expected);
		Mapper mapper = new Mapper();
		mapper.RegisterType<ObjectWithValue<float>>();

		var actual = mapper.Map<ObjectWithValue<float>, ObjectWithValue<float>>(objectWithValue).Value;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Map_DoubleToDouble_ShouldMap() 
	{
		var expected = 1.0;
		ObjectWithValue<double> objectWithValue = new ObjectWithValue<double>(expected);
		Mapper mapper = new Mapper();
		mapper.RegisterType<ObjectWithValue<double>>();

		var actual = mapper.Map<ObjectWithValue<double>, ObjectWithValue<double>>(objectWithValue).Value;

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Map_BooleanToBoolean_Should_Map() 
	{
		var expected = true;
		ObjectWithValue<bool> objectWithValue = new ObjectWithValue<bool>(expected);
		Mapper mapper = new Mapper();
		mapper.RegisterType<ObjectWithValue<bool>>();

		var actual = mapper.Map<ObjectWithValue<bool>, ObjectWithValue<bool>>(objectWithValue).Value;

		Assert.Equal(expected, actual);
	}
}