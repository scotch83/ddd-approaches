using System;
using System.Timers;
using System.Collections.Generic;

public class Downstream
{

	// A constructor for a domain specific class

	public Downstream()
	{
		SetupTimer();
	}

	// A static collection mapping domains to their busses for message sending

	static Dictionary<Type, Action<DownstreamMessage>> ComBusCollection = new Dictionary<Type, Action<DownstreamMessage>>();

	// A function that is adding mapping functionality within exposed concept (DownstreamMessage) and external world specific types T

	public static void SetComBusToUpstream<T>(Action<T> onNext, Func<DownstreamMessage, T> converter)
	{
		ComBusCollection.Add(typeof(T), downMessage => onNext.Invoke(converter(downMessage)));
	}

	// A domain specific type that is exposed to the outer world

	public class DownstreamMessage
	{
		public string Message { get; set; }
	}

	// setup a timer to send messages to subscribed listeners
	// this approach does not take care of communicating previous state to listeners subscribing after the events have been sent
	// something like Rx Subjects can take care of this kind of things (or our own implementation of it)

	void SetupTimer()
	{
		var timer = new Timer(1000);
		var counter = 1;

		timer.Elapsed += (obj, e) => {
			foreach (var item in ComBusCollection)
				item.Value.Invoke(new DownstreamMessage { Message = "This is an emitted message " + counter++ });
		};

		timer.Enabled = true;
	}

	public void SendMessageInSystem(DownstreamMessage msg)
	{
		Console.WriteLine("I have received another message from someone " + msg.Message);
	}
}
