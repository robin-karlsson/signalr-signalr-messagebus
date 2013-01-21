using System;
using System.Globalization;
using Microsoft.AspNet.SignalR;

namespace Chat2.Hubs
{
	public class ChatHub : Hub
	{
		private const string Format = "{0} from {2}Hub 2 ({1})";

		public void SendMessage(string message)
		{
			Clients.All.append(string.Format(CultureInfo.InvariantCulture, Format, message, DateTime.UtcNow, ""));
		}

		public string[] AvailableGroups()
		{
			return new[] {"SignalR", "ASP.Net MVC"};
		}

		public void JoinGroups(string[] groups)
		{
			foreach (var group in groups)
			{
				Groups.Add(Context.ConnectionId, group);
			}
		}

		public void MessageGroup(string group, string message)
		{
			Clients.Group(group).append(string.Format(CultureInfo.InvariantCulture, Format, message, DateTime.UtcNow,
														   group + "/"));
		}

		public void LeaveGroups(string[] groups)
		{
			foreach (var group in groups)
			{
				Groups.Remove(Context.ConnectionId, group);
			}
		}
	}
}