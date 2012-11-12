using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR;

namespace Contrib.SignalR.SignalRMessageBus.Backend
{
	public class SignalRBackplane : PersistentConnection
	{
		private static readonly object lockobj = new object();
		private static readonly IDictionary<long,string> messageDictionary = new ConcurrentDictionary<long, string>();
		private static Timer timer;

		public override void Initialize(IDependencyResolver resolver, HostContext context)
		{
			lock (lockobj)
			{
				if (timer==null)
				{
					timer = new Timer(scavageDictionary,null,TimeSpan.FromSeconds(60),TimeSpan.FromSeconds(60));
				}
			}
			base.Initialize(resolver, context);
		}

		private static void scavageDictionary(object state)
		{
			if (messageDictionary.Count<100000)
				return;

			lock (lockobj)
			{
				var lastIdToSave = getLastId() - 100000;
				var keysToRemove = messageDictionary.Keys.Where(k => k < lastIdToSave).ToArray();

				foreach (var key in keysToRemove)
				{
					messageDictionary.Remove(key);
				}
			}
		}

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

		private static long getLastId()
		{
			if (messageDictionary.Count == 0) return 0L;
			return messageDictionary.Keys.Max();
		}
	}
}