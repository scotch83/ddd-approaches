using System;

namespace AnotherDomain
{
	public class AnotherDomainClass
	{
		public AnotherDomainMessage GetAnotherDomainMessage()
		{
			return new AnotherDomainMessage();
		}
	}

	public class AnotherDomainMessage
	{
		public string Name = "Hello Kitty";
	}
}
