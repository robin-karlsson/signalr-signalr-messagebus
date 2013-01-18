using System.Web;
using System.Web.Routing;

[assembly: PreApplicationStartMethod(typeof(Contrib.SignalR.SignalRMessageBus.Backend.RegisterConnection), "Start")]

namespace Contrib.SignalR.SignalRMessageBus.Backend
{
    public static class RegisterConnection
    {
        public static void Start()
        {
			RouteTable.Routes.MapConnection<SignalRBackplane>("backplane", "backplane");
        }
    }
}