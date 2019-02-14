using System;
using static Downstream;
using static Upstream;

namespace TestConsole
{
	public static class UpstreamExtensionComLayer
	{
		public static void MapDownstreamToUpstream(this Upstream upStream, Func<UpstreamMessage> convertMessageForStream)
		{
			upStream.OnNext(convertMessageForStream());
		}
	}
}
