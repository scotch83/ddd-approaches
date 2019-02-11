using static Upstream;
using System;

namespace TestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var t1 = new Downstream();
			var t2 = new Upstream();

			t2.MapDownstreamToUpstream(t1.GetAMessage());
			Console.WriteLine();
		}
	}
}


