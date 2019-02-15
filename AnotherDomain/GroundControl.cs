using System;
using System.Timers;
public class GroundControl
{
	public event Action<GroundControlCommunication> MessageSent;
	public GroundControl()
	{
		var timer = new Timer(2000);
		timer.Elapsed += Timer_Elapsed;
		timer.Enabled = true;
	}

	void Timer_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (MessageSent == null)
			Console.WriteLine("Do we want to send this message? Subscribing afterwards will loose data that is not existing anymore...");

		else MessageSent.Invoke(GetMessageFromGroundControl());
	}


	public GroundControlCommunication GetMessageFromGroundControl()
	{
		return new GroundControlCommunication();
	}
}

public class GroundControlCommunication
{
	public string APropertyToMap = "Ground control to Major Tom!";
}
