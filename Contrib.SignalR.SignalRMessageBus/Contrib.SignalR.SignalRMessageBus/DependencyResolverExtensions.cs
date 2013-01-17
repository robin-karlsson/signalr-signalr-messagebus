using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;

namespace Contrib.SignalR.SignalRMessageBus
{
    public static class DependencyResolverExtensions
    {
		public static IDependencyResolver UseSignalRServer(this IDependencyResolver resolver, Uri serverUri)
        {
            var bus = new Lazy<SignalRMessageBus>(() => new SignalRMessageBus(serverUri, resolver));
            resolver.Register(typeof(IMessageBus), () => bus.Value);

            return resolver;
        }
    }
}
