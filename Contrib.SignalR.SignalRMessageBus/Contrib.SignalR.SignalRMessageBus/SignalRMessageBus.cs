using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Messaging;
using Connection = Microsoft.AspNet.SignalR.Client.Connection;

namespace Contrib.SignalR.SignalRMessageBus
{
    public class SignalRMessageBus : ScaleoutMessageBus
    {
    	private readonly Connection _connection;
    	private Task startTask;
	    private const int StreamIndex = 0;

	    public SignalRMessageBus(SignalRScaleoutConfiguration scaleoutConfiguration, IDependencyResolver dependencyResolver)
			: base(dependencyResolver, scaleoutConfiguration)
        {
			_connection = new Connection(scaleoutConfiguration.ServerUri.ToString());
    		_connection.Received += notificationRecieved;
			_connection.Error += e =>
				{
					Debug.WriteLine(e.ToString());
					OnError(0, e);
				};
    		startTask = _connection.Start();
		    startTask.ContinueWith(t =>
			    {
				    throw t.Exception.GetBaseException();
			    }, TaskContinuationOptions.OnlyOnFaulted);
		    startTask.ContinueWith(_ =>
			    {
				    Open(StreamIndex);
			    }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

    	private void notificationRecieved(string obj)
    	{
    		var indexOfFirstHash = obj.IndexOf('#');
    		var message = obj.Substring(indexOfFirstHash + 3).ToScaleoutMessage();

			if (message.Messages == null || message.Messages.Count == 0)
			{
				Open(StreamIndex);
			}

			OnReceived(StreamIndex, (ulong)Convert.ToInt64(obj.Substring(0, indexOfFirstHash)), message);
    	}

		protected override Task Send(IList<Message> messages)
        {
			if (messages == null || messages.Count == 0)
			{
				var emptyTask = new TaskCompletionSource<object>();
				emptyTask.SetResult(null);
				return emptyTask.Task;
			}

			if (_connection.State == ConnectionState.Disconnected)
			{
				startTask = _connection.Start();
				startTask.ContinueWith(t =>
				{
					throw t.Exception.GetBaseException();
				}, TaskContinuationOptions.OnlyOnFaulted);
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
