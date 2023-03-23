"use strict";

var MessageViewModel = function () {

    var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    var self = this;
    self.userName = ko.observable(document.getElementById("userInput").value);
    self.userMessage = ko.observable("");
    self.message = ko.observable("");  
    self.messagesList = ko.observableArray();

    connection.on("ReceiveMessage", function (user, message) {
        var mes = user == "" ? message : user + ": " + message;
        self.messagesList.push({ message: mes });
    });

    connection.start().then(function () {
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.onclose(function () {
        console.log('connecition closed');
        self.messagesList.push({ message: "Потеряно соединение с сервером..." });
    });

    self.send = function () {
        connection.invoke("SendMessage", self.userName(), self.userMessage()).catch(function (err) {
            return console.error(err.toString());
        });
        self.userMessage("");
    }
}

ko.applyBindings(new MessageViewModel());