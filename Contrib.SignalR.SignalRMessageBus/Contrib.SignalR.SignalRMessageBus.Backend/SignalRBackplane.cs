using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hosting;

namespace Contrib.SignalR.SignalRMessageBus.Backend
{
	public class SignalRBackplane : PersistentConnection
	{
		private static readonly object lockobj = new object();
		private static readonly IDictionary<ulong,string> messageDictionary = new ConcurrentDictionary<ulong, string>();
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
				var lastIdToSave = IdStorage.LastId - 100000;
				var keysToRemove = messageDictionary.Keys.Where(k => k < lastIdToSave).ToArray();

				foreach (var key in keysToRemove)
				{
					messageDictionary.Remove(key);
				}
			}
		}

		protected override System.Threading.Tasks.Task OnReceived(IRequest request, string connectionId, string data)
		{
			ulong id;
			lock (lockobj)
			{
				id = IdStorage.LastId++;
				messageDictionary.Add(id, data);
			}

			return Connection.Broadcast(id + "#"+data);
		}
	}
}