using System;
using System.Globalization;
using Microsoft.AspNet.SignalR;

namespace Chat2.Hubs
{
	public class ChatHub : Hub
	{
		private const string Format = "{0} from Hub 2 ({1})";

		public void SendMessage(string message)
		{
			Clients.All.append(string.Format(CultureInfo.InvariantCulture, Format, message, DateTime.UtcNow));
		}
	}
}