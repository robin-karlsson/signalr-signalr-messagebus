using System;
using Microsoft.AspNet.SignalR.Messaging;

namespace Contrib.SignalR.SignalRMessageBus
{
	public class SignalRScaleoutConfiguration : ScaleoutConfiguration
	{
		public SignalRScaleoutConfiguration(Uri serverUri)
		{
			ServerUri = serverUri;
		}

		public Uri ServerUri { get; set; }
	}
}