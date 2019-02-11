using System;
using static Downstream;
using static Upstream;

namespace TestConsole
{
	public static class UpstreamExtensionComLayer
	{
		public static void MapDownstreamToUpstream(this Upstream up, DownstreamMessage downstream)
		{
			var upstreamMsg = new UpstreamMessage { Message = downstream.Message };
			up.OnNext(upstreamMsg);
		}
	}
}
