using System;

namespace Mapping;

public class Person
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string FirstName { get; set; }
	public string LastName { get; set; }

	public Person(Guid id, string firstName, string lastName)
	{
		Id = id;
		FirstName = firstName;
		LastName = lastName;
	}

	public Person(string firstName,  string lastName)
	{
		FirstName = firstName;
		LastName = lastName;
	}

	public Person()
	{

	}
}
