using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNet.SignalR.Messaging;

namespace Contrib.SignalR.SignalRMessageBus
{
	public static class ScaleoutMessageExtensions
	{
		public static string ToScaleoutString(this IList<Message> messages)
		{
			if (messages == null)
			{
				throw new ArgumentNullException("messages");
			}

			var message = new ScaleoutMessage(messages);
			return Convert.ToBase64String(message.ToBytes());
		}

		public static ScaleoutMessage ToScaleoutMessage(this string stringMessage)
		{
			var message = ScaleoutMessage.FromBytes(Convert.FromBase64String(stringMessage));

			return message;
		}
	}
}