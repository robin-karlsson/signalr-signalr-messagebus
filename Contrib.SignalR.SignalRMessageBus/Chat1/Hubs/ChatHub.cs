using System;
using System.Globalization;
using Microsoft.AspNet.SignalR;

namespace Chat1.Hubs
{
	public class ChatHub : Hub
	{
		private const string Format = "{0} from {2}Hub 1 ({1})";

		public void SendMessage(string message)
		{
			Clients.All.append(string.Format(CultureInfo.InvariantCulture, Format, message, DateTime.UtcNow, ""));
		}

		public string[] AvailableGroups()
		{
			return new[] { "SignalR", "ASP.Net MVC" };
		}

		public void JoinGroup(string group)
		{
			Groups.Add(Context.ConnectionId, group);
		}

		public void MessageGroup(string group, string message)
		{
			Clients.Group(group).append(string.Format(CultureInfo.InvariantCulture, Format, message, DateTime.UtcNow,
														   group + "/"));
		}

		public void LeaveGroup(string group)
		{
			Groups.Remove(Context.ConnectionId, group);
		}
	}
}