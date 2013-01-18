SignalR backplane for scaling out SignalR
==========================

Add the backend part to your desired master web site, where you want the backplane to run. Then add the message bus part to your SignalR site and use the extension method to set the proper URL to the published backplane service.