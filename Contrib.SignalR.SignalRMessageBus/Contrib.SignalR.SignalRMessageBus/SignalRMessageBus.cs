using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Contrib.SignalR.SignalRMessageBus
{
    public class SignalRMessageBus : ScaleoutMessageBus
    {
        private readonly SignalRSender _sender;
        private readonly SignalRReceiver _receiver;

        public SignalRMessageBus(string serverUrl, IDependencyResolver dependencyResolver)
            : this(serverUrl, null, null, dependencyResolver)
        {
        }

        internal SignalRMessageBus(string serverUrl, SignalRSender signalRSender, SignalRReceiver signalRReceiver, IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            _sender = signalRSender ?? new SignalRSender(serverUrl);
            _receiver = signalRReceiver ?? new SignalRReceiver(serverUrl, OnReceived);
        }

        protected override Task Send(Message[] messages)
        {
            return _sender.Send(messages);
        }

        public override void Dispose()
        {
            _receiver.Dispose();
            base.Dispose();
        }
    }
}
