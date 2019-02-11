﻿using System.Reactive.Subjects;
using System;

public class Downstream
{

	public DownstreamMessage DownStreamSpecificData { get; set; }

	public BehaviorSubject<DownstreamMessage> LatestPrimitivesMessageeStreamPlug = new BehaviorSubject<DownstreamMessage>(null);

	public Downstream()
	{
		LatestPrimitivesMessageeStreamPlug.Subscribe(data => {
			DownStreamSpecificData = data ?? new DownstreamMessage { Message = "I have no messages for you" };
			Console.WriteLine(DownStreamSpecificData.Message);
		});
	}

	public override string ToString()
	{
		return DownStreamSpecificData.Message;
	}

	// this is just to track difference in sent messages
	static int count = 1;

	public DownstreamMessage GetAMessage()
	{
		return new DownstreamMessage { Message = $"This is a message {count++}" };
	}

	public class DownstreamMessage
	{
		public string Message { get; set; }	
	}

	public class DownstreamACL
	{

		// depending on the kind of state that we want
		public readonly BehaviorSubject<DownstreamMessage> LastMessageStream = new BehaviorSubject<DownstreamMessage>(null);

		// Singleton
		static DownstreamACL _instance;

		public static DownstreamACL Instance
		{
			get
			{
				return _instance = _instance ?? new DownstreamACL();
			}
		}

		private void SendMessage(DownstreamMessage msg)
		{
			LastMessageStream.OnNext(msg);
		}

	}
}