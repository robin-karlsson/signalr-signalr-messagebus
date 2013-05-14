<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Chat1._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="Scripts/jquery-2.0.0.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.signalR-1.1.0.min.js" type="text/javascript"></script>
    <script src="signalr/hubs" type="text/javascript"></script>
    <script src="Scripts/knockout-2.2.1.debug.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            var model = function (hub) {
                var self = this;

                this.messages = ko.observableArray();
                this.sendEnabled = ko.observable(false);
                this.messageFocus = ko.observable(false);
                this.message = ko.observable();
                this.selectedTargetGroup = ko.observable();
                this.selectedGroups = ko.observableArray();
                this.availableGroups = ko.observableArray();
                this.errorMessage = ko.observable();

                this.send = function () {
                    var val = self.message();
                    if (val || val != "") {
                        var promise;
                        var selectedGroup = self.selectedTargetGroup();
                        if (!selectedGroup) {
                            promise = hub.server.sendMessage(val);
                        }
                        else {
                            promise = hub.server.messageGroup(selectedGroup, val);
                        }
                        promise.done(function () {
                            self.message('');
                            self.messageFocus(true);
                        });
                    }
                };

                this.getAvailableGroups = function () {
                    hub.server.availableGroups().done(function (groups) {
                        ko.utils.arrayForEach(groups, function (g) {
                            self.availableGroups.push(g);
                        });
                    });
                };

                hub.client.append = function (message) {
                    self.messages.push(message);
                };
            };

            var myHub = $.connection.chatHub;
            var vm = new model(myHub);

            $.connection.hub.error(function () {
                vm.errorMessage("An error occured");
            });

            vm.selectedGroups.subscribe(function () {
                myHub.server.leaveGroups(vm.availableGroups()).done(function () {
                    myHub.server.joinGroups(vm.selectedGroups());
                });
            });

            $.connection.hub.start()
                .done(function () {
                    vm.sendEnabled(true);

                    vm.getAvailableGroups();

                    vm.messageFocus(true);
                })
                .fail(function () {
                    vm.errorMessage("Could not Connect!");
                });

            ko.applyBindings(vm);
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to Chat1!
    </h2>
        <h3>Send</h3>
    <p>
        <input type="text" placeholder="Enter your message here!" data-bind="value: message, hasfocus: messageFocus"/>
        <select data-bind='options: availableGroups, optionsCaption: "To all", value: selectedTargetGroup'></select>
        <input type="button" value="Send!" data-bind="enabled: sendEnabled, click: send"/>
    </p>
    <hr/>
        <h3>Subscribe to group</h3>
    <p data-bind="foreach: availableGroups">
        <input type="checkbox" data-bind="attr: { value: $data }, checked: $root.selectedGroups"><span data-bind="text: $data"></span><br/>
    </p>
    <hr/>
        <ul data-bind="foreach: messages">
            <li data-bind="text: $data"></li>
        </ul>
</asp:Content>
