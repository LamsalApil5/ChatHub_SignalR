"use strict";

var userIpAddress = "@userIpAddress";
var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

// Disable the send button until the connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (ipAddress, message) {
    var li = document.createElement("li");
    if (ipAddress === userIpAddress) {
        li.className = "p-2 my-1 bg-secondary text-white rounded text-end";
    } else {
        li.className = "p-2 my-1 bg-light text-dark rounded text-start";
    }
    li.textContent = message;
    document.getElementById("messagesList").appendChild(li);

    // Auto-scroll to the latest message
    document.getElementById("messagesList").scrollTop = document.getElementById("messagesList").scrollHeight;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", userIpAddress, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();

    // Clear the message input field after sending
    document.getElementById("messageInput").value = '';
});

document.addEventListener("DOMContentLoaded", function () {
    var sendButton = document.getElementById("sendButton");

    // Apply initial styles
    sendButton.style.backgroundColor = "#007bff"; // Bootstrap primary blue
    sendButton.style.color = "white";
    sendButton.style.border = "none";
    sendButton.style.padding = "10px 20px";
    sendButton.style.fontSize = "16px";
    sendButton.style.borderRadius = "4px";
    sendButton.style.cursor = "pointer";
    sendButton.style.transition = "background-color 0.3s ease";

    // Hover effect
    sendButton.addEventListener("mouseover", function () {
        sendButton.style.backgroundColor = "#0056b3"; // Darker shade of blue
    });

    sendButton.addEventListener("mouseout", function () {
        sendButton.style.backgroundColor = "#007bff"; // Bootstrap primary blue
    });
});