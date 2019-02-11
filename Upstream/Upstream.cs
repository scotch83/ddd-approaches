using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Upstream : IObserver<UpstreamMessage>
{
	public IObservable<UpstreamMessage> LatestPrimitivesMessageeStreamPlug = new UpstreamMessageQueue();

	public UpstreamMessage UpstreamSpecificData { get; set; }

	public Upstream()
	{
		LatestPrimitivesMessageeStreamPlug.Subscribe(this);
	}
	public override string ToString()
	{
		return UpstreamSpecificData.Message;
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
		UpstreamSpecificData = value;
		Debug.WriteLine(UpstreamSpecificData.Message);
	}
}

public class UpstreamMessage
{
	public string Message { get; set; }
}

public class UpstreamMessageQueue : IObservable<UpstreamMessage>
{
	List<IObserver<UpstreamMessage>> _observers = new List<IObserver<UpstreamMessage>>();
	public IDisposable Subscribe(IObserver<UpstreamMessage> observer)
	{
		_observers.Add(observer);
		return new Unsubscriber(_observers, observer);
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

	public void PostMessage(UpstreamMessage message)
	{
		foreach (var observer in _observers)
			observer.OnNext(message);
	}

}
