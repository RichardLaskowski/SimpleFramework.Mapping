using System;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

namespace Mapping;
internal class Program
{
	public static void Main(string[] args)
	{
		System.Console.WriteLine("What are you going to do this evening.");
	}

	public class User
	{
		public Guid Id { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
