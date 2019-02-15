using System;
using System.Collections.Generic;
using System.Timers;


// This approach use the observer pattern to track changes
// It is a custom implementation of IObserver and IObservabla, but can take advantage of Subjects, an Rx concept (see: http://reactivex.io/documentation/subject.html)

public class Upstream : IObserver<UpstreamMessage>, IObservable<UpstreamMessage>
{

	public UpstreamMessageQueue LatestPrimitivesMessageeStreamPlug = new UpstreamMessageQueue();

	public Upstream()
	{
		LatestPrimitivesMessageeStreamPlug.Subscribe(this);
		SetupTimer();
	}

	
	// setup a timer to send messages to subscribed listeners
	// this approach does not take care of communicating previous state to listeners subscribing after the events have been sent
	// something like Rx Subjects can take care of this kind of things (or our own implementation of it)

	void SetupTimer()
	{
		var timer = new Timer(1000);
		var counter = 1;

		timer.Elapsed += (obj, e) => UpstreamMessageQueue.PostMessage(new UpstreamMessage { Message = " This is an upstream message " + counter++ });	

		timer.Enabled = true;
	}

	public void SendMessageInSystem(UpstreamMessage msg)
	{
		OnNext(msg);
	}

	public void OnCompleted()
	{
	}

	public void OnError(Exception error)
	{
		throw error;
	}

	public void OnNext(UpstreamMessage value)
	{
		Console.WriteLine("I received a message from someone I am observing and that is " + value.Message);
	}

	public IDisposable Subscribe(IObserver<UpstreamMessage> observer)
	{
		return LatestPrimitivesMessageeStreamPlug.Subscribe(observer);
	}
}

public class UpstreamMessage
{
	public string Message { get; set; }
}

public class UpstreamMessageQueue : IObservable<UpstreamMessage>
{
	static List<IObserver<UpstreamMessage>> _observers = new List<IObserver<UpstreamMessage>>();

	public IDisposable Subscribe(IObserver<UpstreamMessage> observer)
	{
		_observers.Add(observer);
		return new Unsubscriber(_observers, observer);
	}


	public static void PostMessage(UpstreamMessage message)
	{
		foreach (var observer in _observers)
			observer.OnNext(message);
	}

	private class Unsubscriber : IDisposable
	{
		readonly List<IObserver<UpstreamMessage>> _observers;
		readonly IObserver<UpstreamMessage> _observer;
		public Unsubscriber(List<IObserver<UpstreamMessage>> observers, IObserver<UpstreamMessage> observer)
		{
			_observers = observers;
			_observer = observer;
		}
		
		public void Dispose()
		{
			if (_observer != null && _observers.Contains(_observer))
				_observers.Remove(_observer);
		}
	}
}
