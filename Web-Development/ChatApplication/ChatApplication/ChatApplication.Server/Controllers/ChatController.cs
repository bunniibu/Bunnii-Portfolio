using ChatApplication.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Server.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;
    private readonly UserService _userService;

    public ChatController(ChatService chatService, UserService userService)
    {
        _chatService = chatService;
        _userService = userService;
    }
    
    [HttpPost("register")]
    public IActionResult RegisterUser([FromBody] UserDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Nickname))
            return BadRequest("Nickname cannot be empty.");

        if (_userService.IsNicknameTaken(request.Nickname))
            return BadRequest("Nickname already taken.");

        return _userService.RegisterUser(request.Nickname) ? Ok() : BadRequest("Registration failed.");
    }
    
    [HttpGet("rooms")]
    public IActionResult GetRooms()
    {
        var roomNames = _chatService.GetRooms();
        return Ok(roomNames);
    }

    
    [HttpPost("rooms/create")]
    public IActionResult CreateRoom([FromBody] string roomName)
    {

        if (string.IsNullOrWhiteSpace(roomName))
        {
            return BadRequest("Room name cannot be empty.");
        }

        _chatService.CreateRoom(roomName);
        return Ok();
    }



    [HttpDelete("rooms/delete/{roomName}")]
    public IActionResult DeleteRoom(string roomName)
    {

        if (_chatService.DeleteChatRoom(roomName, out string errorMessage))
        {
            return Ok($"Room'{roomName}' was succesfully deleted.");
        }

        return BadRequest(errorMessage); 
    }


 
    [HttpPost("rooms/join")]
    public IActionResult JoinRoom([FromBody] JoinRoomRequest request)
    {
        _chatService.JoinRoom(request.RoomName, request.Nickname);
        return Ok($"User '{request.Nickname}' joined room '{request.RoomName}'.");
    }

   
    [HttpPost("rooms/leave")]
    public IActionResult LeaveRoom([FromBody] LeaveRoomRequest request)
    {
        _chatService.LeaveRoom(request.RoomName, request.Nickname);
        return Ok($"User '{request.Nickname}' left room '{request.RoomName}'.");
    }


    [HttpGet("rooms/{roomName}/users")]
    public IActionResult GetUsersInRoom(string roomName)
    {
        var users = _chatService.GetUsersInRoom(roomName);
        return Ok(users);
    }

    public class JoinRoomRequest
    {
        public string Nickname { get; set; }
        public string RoomName { get; set; }
    }

    public class LeaveRoomRequest
    {
        public string Nickname { get; set; }
        public string RoomName { get; set; }
    }
    public class UserDto
    {
        public string Nickname { get; set; }
    }
}