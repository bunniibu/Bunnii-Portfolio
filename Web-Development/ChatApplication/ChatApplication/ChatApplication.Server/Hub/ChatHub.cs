using System.Collections.Concurrent;
using ChatApplication.Server.Services;

namespace ChatApplication.Server.Hub;
using Microsoft.AspNetCore.SignalR;
public class ChatHub : Hub
{
    private readonly ChatService _chatService;

    public ChatHub(ChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task JoinRoom(string roomName, string user)
    {
        _chatService.JoinRoom(roomName, user);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);


        var users = _chatService.GetUsersInRoom(roomName);
        await Clients.Group(roomName).SendAsync("UpdateUserList", users);
    }

    public async Task LeaveRoom(string roomName, string user)
    {
        _chatService.LeaveRoom(roomName, user);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);


        var users = _chatService.GetUsersInRoom(roomName);
        await Clients.Group(roomName).SendAsync("UpdateUserList", users);
    }

    public async Task SendMessage(string roomName, string user, string message)
    {
        await Clients.Group(roomName).SendAsync("ReceiveMessage", roomName, user, message);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
