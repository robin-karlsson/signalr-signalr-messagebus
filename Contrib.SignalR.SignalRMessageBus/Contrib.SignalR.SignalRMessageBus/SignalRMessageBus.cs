using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;
using Newtonsoft.Json;
using Connection = Microsoft.AspNet.SignalR.Client.Connection;

namespace Contrib.SignalR.SignalRMessageBus
{
    public class SignalRMessageBus : ScaleoutMessageBus
    {
    	private readonly Connection _connection;
    	private readonly Task startTask;

    	public SignalRMessageBus(Uri serverUri, IDependencyResolver dependencyResolver) : base(dependencyResolver)
        {
			_connection = new Connection(serverUri.ToString());
    		_connection.Received += notificationRecieved;
    		startTask = _connection.Start();
			startTask.ContinueWith(t => { throw t.Exception.GetBaseException(); }, TaskContinuationOptions.OnlyOnFaulted);
        }

    	private void notificationRecieved(string obj)
    	{
    		var indexOfFirstHash = obj.IndexOf('#');
    		OnReceived("0", (ulong) Convert.ToInt64(obj.Substring(0, indexOfFirstHash)),
    		           JsonConvert.DeserializeObject<Message[]>(obj.Substring(indexOfFirstHash+3)));
    	}

    	protected override Task Send(Message[] messages)
        {
			if (messages == null || messages.Length == 0)
			{
				var emptyTask = new TaskCompletionSource<object>();
				emptyTask.SetResult(null);
				return emptyTask.Task;
			}

			if (!startTask.IsCompleted)
			{
				startTask.Wait();
			}

			return _connection.Send("s:"+JsonConvert.SerializeObject(messages));
        }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_connection.Stop();
			}
		}
    }
}
