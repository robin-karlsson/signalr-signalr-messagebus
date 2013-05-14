﻿using System;
using System.Web.Routing;
using Contrib.SignalR.SignalRMessageBus;
using Microsoft.AspNet.SignalR;

namespace Chat2
{
	public class Global : System.Web.HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			// Code that runs on application startup
			RouteTable.Routes.MapHubs();
			GlobalHost.DependencyResolver.UseSignalRServer(new Uri("http://localhost:56715/backplane"));
		}

		void Application_End(object sender, EventArgs e)
		{
			//  Code that runs on application shutdown
		}

		void Application_Error(object sender, EventArgs e)
		{
			// Code that runs when an unhandled error occurs
		}

		void Session_Start(object sender, EventArgs e)
		{
			// Code that runs when a new session is started
		}

		void Session_End(object sender, EventArgs e)
		{
			// Code that runs when a session ends. 
			// Note: The Session_End event is raised only when the sessionstate mode
			// is set to InProc in the Web.config file. If session mode is set to StateServer 
			// or SQLServer, the event is not raised.
		}
	}
}
