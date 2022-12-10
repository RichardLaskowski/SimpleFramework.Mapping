using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFramework.Mapping.Tests.TestObjects;
public class ObjectWithGuid
{
	public Guid Value { get; set; }

	public ObjectWithGuid()
	{

	}

	public ObjectWithGuid(Guid value)
	{
		Value = value;
	}
}
