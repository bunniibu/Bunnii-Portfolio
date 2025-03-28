using System.Collections.Concurrent;
using ChatApplication.Server.Hub;
using ChatApplication.Server.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Server.Services;

public class ChatService
{
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatService(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
        chatRooms["General"] = new HashSet<string>();
    }
    private readonly ConcurrentDictionary<string, HashSet<string>> chatRooms = new(); 

    

    public List<string> GetRooms()
    {
        return chatRooms.Keys.ToList();
    }

    
    public void JoinRoom(string roomName, string user)
    {
        if (!chatRooms.ContainsKey(roomName))
        {
            return; 
        }

        if (roomName == user)
        {
            return; 
        }

        chatRooms[roomName].Add(user);

       
        _hubContext.Clients.Group(roomName).SendAsync("ReceiveMessage", roomName, "System", $"{user} has joined the chat!");
    }






    public void LeaveRoom(string roomName, string user)
    {
        if (!chatRooms.ContainsKey(roomName))
        {
            return; 
        }

        chatRooms[roomName].Remove(user);
        
        _hubContext.Clients.Group(roomName).SendAsync("ReceiveMessage", roomName, "System", $"{user} has left the chat.");
    }




    public bool DeleteChatRoom(string roomName, out string errorMessage)
    {

        if (!chatRooms.ContainsKey(roomName))
        {
            errorMessage = "Room ddoesn't exist.";
            return false;
        }

        if (chatRooms[roomName].Count > 0)
        {
            errorMessage = $"The room '{roomName}' can't be deleted since there are {chatRooms[roomName].Count} users online";
            return false;
        }

        chatRooms.Remove(roomName, out _);
        errorMessage = "";
        return true;
    }


    
    public void CreateRoom(string roomName)
    {
        if (!chatRooms.ContainsKey(roomName))
        {
            chatRooms[roomName] = new HashSet<string>(); 
        }
    }
    
    public List<string> GetUsersInRoom(string roomName)
    {
        if (chatRooms.ContainsKey(roomName))
        {
            return chatRooms[roomName].ToList();
        }

        return new List<string>(); 
    }

}