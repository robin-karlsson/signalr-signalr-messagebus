using System.Web;
using System.Web.Routing;
using Chat.Backplane;
using Contrib.SignalR.SignalRMessageBus.Backend;

[assembly: PreApplicationStartMethod(typeof(RegisterConnection), "Start")]

namespace Chat.Backplane
{
    public static class RegisterConnection
    {
        public static void Start()
        {
			RouteTable.Routes.MapConnection<SignalRBackplane>("backplane", "backplane");
        }
    }
}
