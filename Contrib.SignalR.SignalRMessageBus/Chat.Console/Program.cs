using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Chat.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var connection = new HubConnection("http://localhost:57459/");
			connection.TraceLevel = TraceLevels.All;
			connection.TraceWriter = System.Console.Out;
			var proxy = connection.CreateHubProxy("ChatHub");
			connection.Start()
				.ContinueWith(t =>
					{
						throw t.Exception.GetBaseException();
					},TaskContinuationOptions.OnlyOnFaulted)
				.ContinueWith(_ =>
				{
					for (int i = 0; i < 1000; i++)
					{
						Thread.Sleep(TimeSpan.FromMilliseconds(new Random().Next(1, 15)));
						var message = string.Format("Message number {0}", i);

						System.Console.WriteLine(message);
						proxy.Invoke("MessageGroup", "SignalR", message).Wait();
					}
				}).ContinueWith(_ => connection.Stop());
			System.Console.ReadLine();
		}
	}
}
