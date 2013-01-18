The backplane must be mapped correctly to an url before using it.
Below is the sample code for that, taken from the Chat.Backplane sample.

RouteTable.Routes.MapConnection<SignalRBackplane>("backplane", "backplane");