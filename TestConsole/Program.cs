using static Upstream;
using System;
using static Downstream;
using AnotherDomain;

namespace TestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var domain1 = new Upstream();
			var domain2 = new Downstream();
			var domain3 = new AnotherDomainClass();

			domain1.MapDownstreamToUpstream(() => new UpstreamMessage { Message = domain2.GetAMessage().Message });
			domain1.MapDownstreamToUpstream(() => new UpstreamMessage { Message = domain3.GetAnotherDomainMessage().Name });
			domain2.SetComBusToUpstream(domain1.OnNext, msg => new UpstreamMessage { Message = msg.Message });

			Console.ReadLine();
		}
	}
}
