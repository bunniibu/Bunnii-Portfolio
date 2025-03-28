let connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

function sendMessage() {
    let message = document.getElementById("messageText").value;
    let user = "user";
    alert(`${user}: ${message}`);
}

