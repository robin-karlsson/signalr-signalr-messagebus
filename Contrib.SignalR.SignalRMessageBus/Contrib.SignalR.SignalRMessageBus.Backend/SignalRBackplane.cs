using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;

namespace Contrib.SignalR.SignalRMessageBus.Backend
{
	public class SignalRBackplane : PersistentConnection
	{
		private static readonly object lockobj = new object();
		private static readonly IDictionary<long,string> messageDictionary = new ConcurrentDictionary<long, string>();

		protected override System.Threading.Tasks.Task OnReceivedAsync(IRequest request, string connectionId, string data)
		{
			long id;
			lock (lockobj)
			{
				id = getLastId() + 1;
				messageDictionary.Add(id,data);
			}
			return Connection.Broadcast(id + "#"+data);
		}

		private long getLastId()
		{
			if (messageDictionary.Count == 0) return 0L;
			return messageDictionary.Keys.Max();
		}
	}
}