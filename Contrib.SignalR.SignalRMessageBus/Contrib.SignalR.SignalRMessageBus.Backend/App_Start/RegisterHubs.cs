using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;

[assembly: PreApplicationStartMethod(typeof(Contrib.SignalR.SignalRMessageBus.Backend.RegisterHubs), "Start")]

namespace Contrib.SignalR.SignalRMessageBus.Backend
{
    public static class RegisterHubs
    {
        public static void Start()
        {
			RouteTable.Routes.MapConnection<SignalRBackplane>("backplane", "backplane/{*operation}");
        }
    }
}
