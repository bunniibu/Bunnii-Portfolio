namespace ChatApplication.Server.Models;

public class ChatRoom
{
    public string Name { get; set; }
    public HashSet<string> Users { get; set; } = new();
}