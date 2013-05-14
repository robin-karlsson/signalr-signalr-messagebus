using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;
using Connection = Microsoft.AspNet.SignalR.Client.Connection;

namespace Contrib.SignalR.SignalRMessageBus
{
    public class SignalRMessageBus : ScaleoutMessageBus
    {
    	private readonly Connection _connection;
    	private readonly Task startTask;

		public SignalRMessageBus(SignalRScaleoutConfiguration scaleoutConfiguration, IDependencyResolver dependencyResolver)
			: base(dependencyResolver, scaleoutConfiguration)
        {
			_connection = new Connection(scaleoutConfiguration.ServerUri.ToString());
    		_connection.Received += notificationRecieved;
			_connection.Error += e => OnError(0, e);
    		startTask = _connection.Start();
			startTask.ContinueWith(t =>
				{
					throw t.Exception.GetBaseException();
				}, TaskContinuationOptions.OnlyOnFaulted);
        }

    	private void notificationRecieved(string obj)
    	{
    		var indexOfFirstHash = obj.IndexOf('#');
    		OnReceived(0, (ulong) Convert.ToInt64(obj.Substring(0, indexOfFirstHash)),
    		           obj.Substring(indexOfFirstHash + 3).ToScaleoutMessage());
    	}

		protected override Task Send(IList<Message> messages)
        {
			if (messages == null || messages.Count == 0)
			{
				var emptyTask = new TaskCompletionSource<object>();
				emptyTask.SetResult(null);
				return emptyTask.Task;
			}

			if (!startTask.IsCompleted)
			{
				startTask.Wait();
			}

			return _connection.Send("s:"+messages.ToScaleoutString());
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
