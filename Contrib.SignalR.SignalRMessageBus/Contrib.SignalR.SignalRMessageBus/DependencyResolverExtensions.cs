using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;

namespace Contrib.SignalR.SignalRMessageBus
{
    public static class DependencyResolverExtensions
    {
		public static IDependencyResolver UseSignalRServer(this IDependencyResolver resolver, SignalRScaleoutConfiguration configuration)
        {
            var bus = new Lazy<SignalRMessageBus>(() => new SignalRMessageBus(configuration, resolver));
            resolver.Register(typeof(IMessageBus), () => bus.Value);

            return resolver;
        }

		public static IDependencyResolver UseSignalRServer(this IDependencyResolver resolver, Uri serverUri)
		{
			return UseSignalRServer(resolver, new SignalRScaleoutConfiguration(serverUri));
		}
    }
}
