using Microsoft.AspNetCore.SignalR.Client;

namespace ChatRoomsLite.ConsoleClient;

public class ClientState
{
    public bool LoggedIn = false;
    public string Username = "";
    public string Token = "";
    public string RoomName = "";
    public string RoomInvitation = "";
    public List<Message> Messages = new List<Message>();
    public HubConnection? Connection;
    public bool WaitingForResponse = false;
}

public class Message
{
    public string Sender = "";
    public string Text = "";
}