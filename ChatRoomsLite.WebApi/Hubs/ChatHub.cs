using ChatRoomsLite.Application.InvitationService;
using ChatRoomsLite.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatRoomsLite.WebApi.Hubs;

public class ChatHub : Hub
{
    private readonly IRoomInvitationService _roomInvitationService;
    
    public ChatHub(IRoomInvitationService roomInvitationService)
    {
        _roomInvitationService = roomInvitationService;
    }

    public override Task OnConnectedAsync()
    {
        Console.WriteLine($"Connection {Context.ConnectionId} connected");
        return base.OnConnectedAsync();
    }
    
    public override Task OnDisconnectedAsync(Exception exception)
    {
        Console.WriteLine($"Connection {Context.ConnectionId} disconnected");
        return base.OnDisconnectedAsync(exception);
    }
    
    public async Task SendMessage(string roomName, string message, string username)
    {
        //await Clients.Group(roomName).SendAsync("Message", message, Context.User?.Claims.FirstOrDefault(c => c.Type == "name")?.Value);
        await Clients.Group(roomName).SendAsync("Message", message, username);
    }

    public async Task JoinRoom(string invitationLink)
    {
        var success = _roomInvitationService.ResolveInvitation(invitationLink, out InvitationModel? invitation);
        if (success)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, invitation!.RoomName);
            await Clients.Caller.SendAsync("Success", invitation.RoomName, "System");
            await Clients.Group(invitation.RoomName).SendAsync("Message", $"{Context.ConnectionId} joined the room", "System");
        }
        await Clients.Caller.SendAsync("Failed", "System", $"Failed to join room {invitationLink}");
    }
}