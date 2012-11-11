using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Connection = Microsoft.AspNet.SignalR.Client.Connection;

namespace Contrib.SignalR.SignalRMessageBus
{
    internal class SignalRSender
    {
    	private readonly Connection _connection;

    	public SignalRSender(string serverUrl)
        {
        	_connection = new Connection(serverUrl);
    		_connection.Start().Wait();
        }

        public Task Send(Message[] messages)
        {
            if (messages == null || messages.Length == 0)
            {
            	var emptyTask = new TaskCompletionSource<object>();
				emptyTask.SetResult(null);
                return emptyTask.Task;
            }
        	return _connection.Send(JsonConvert.SerializeObject(messages));
        }
    }
}
