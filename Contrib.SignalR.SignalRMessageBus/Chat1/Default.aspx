<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Chat1._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="Scripts/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.signalR-1.0.0-rc2.min.js" type="text/javascript"></script>
    <script src="signalr/hubs" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            var myHub = $.connection.chatHub;

            myHub.client.append = function (message) {
                $('#messages').append('<li>' + message + '</li>');
            };

            $.connection.hub.error(function () {
                alert("An error occured");
            });

            $.connection.hub.start()
                .done(function () {
                    var send = $('#send');
                    send.removeAttr('disabled');
                    send.click(function () {
                        var message = $('#message');
                        var val = message.val();
                        if (val || val != "") {
                        	myHub.server.sendMessage(val).done(function() {
                        	    message.val('');
                        	});
                        }
                    	message[0].focus();
                    });

                    $('#message')[0].focus();
                })
                .fail(function () {
                    alert("Could not Connect!");
                });
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to Chat1!
    </h2>
    <p>
        <input id="message" type="text" placeholder="Enter your message here!"/>
        <input id="send" type="button" value="Send!" disabled="disabled"/>
    </p>
        <ul id="messages">
        
        </ul>
</asp:Content>
