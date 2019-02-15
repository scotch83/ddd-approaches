using System;
using static Downstream;

namespace TestConsole
{
	class Program
	{
		static Downstream Down;

		static void Main(string[] args)
		{
			var upstreamDomain = new Upstream();
			Down = new Downstream();
			var domain3 = new GroundControl();

			// This creates an orchestrator that is subscribed subsequently to changes in the upstream, 
			// to notify then the downstream and map the changes to a domain specific model
			OrchestratorObserver orchestrator = new OrchestratorObserver(Down);
			upstreamDomain.Subscribe(orchestrator);

			// Subscribe to events from a domain and map this to another domain
			domain3.MessageSent += msg => upstreamDomain.SendMessageInSystem(new UpstreamMessage { Message = msg.APropertyToMap });

			// Send a message from here. This is a message extracted from a domain and mapped on the fly to another domain
			upstreamDomain.SendMessageInSystem(new UpstreamMessage { Message = domain3.GetMessageFromGroundControl().APropertyToMap });

			// Set in one domain an access point to another domain, with corresponding conversion function to map one type of data to another.
			Downstream.SetComBusToUpstream(upstreamDomain.SendMessageInSystem, msg => new UpstreamMessage { Message = msg.Message });

			// Post a message in the upstream message queue
			UpstreamMessageQueue.PostMessage(new UpstreamMessage { Message = domain3.GetMessageFromGroundControl().APropertyToMap });

			Console.ReadLine();
		}

		class OrchestratorObserver : IObserver<UpstreamMessage>
		{
			readonly Downstream Down;

			public OrchestratorObserver( Downstream downstream)
			{
				Down = downstream;
			}

			void IObserver<UpstreamMessage>.OnCompleted()
			{
			}
			
			void IObserver<UpstreamMessage>.OnError(Exception error)
			{
				throw new NotImplementedException();
			}
			
			void IObserver<UpstreamMessage>.OnNext(UpstreamMessage value)
			{
				Down.SendMessageInSystem(new DownstreamMessage { Message = value.Message });
			}
		}
	}
}
