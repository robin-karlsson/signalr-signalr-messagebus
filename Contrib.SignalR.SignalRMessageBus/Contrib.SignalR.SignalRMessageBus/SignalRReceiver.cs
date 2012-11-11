using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Connection = Microsoft.AspNet.SignalR.Client.Connection;

namespace Contrib.SignalR.SignalRMessageBus
{
    internal class SignalRReceiver: IDisposable
    {
        private readonly Func<string, ulong, Message[], Task> _onReceive;

        private long _lastPayloadId;
    	private readonly Connection _connection;

    	public SignalRReceiver(string serverUrl, Func<string, ulong, Message[], Task> onReceive)
        {
        	_connection = new Connection(serverUrl);
    		_connection.Start().Wait();
            _onReceive = onReceive;

            GetStartingId();
            ListenForMessages();
        }

        public void Dispose()
        {
        }

        private void GetStartingId()
        {

        }

        private void ListenForMessages()
        {
        	GetMessages();
        }

        private Task<bool> GetMessages()
        {
        	var task = new TaskCompletionSource<bool>();
			task.SetResult(true);
        	return task.Task;
        }
    }
}
