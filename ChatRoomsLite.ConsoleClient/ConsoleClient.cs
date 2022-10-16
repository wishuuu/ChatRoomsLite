using ChatRoomsLite.Models;
using ChatRoomsLite.Models.Entities.User.DTOs;
using Flurl.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace ChatRoomsLite.ConsoleClient;

public class ConsoleClient
{
    private ClientState _state;
    private string _serverUrl = "https://localhost:7241/";
    
    public ConsoleClient()
    {
        _state = new ClientState();
    }

    public void NextAction()
    {
        if (!_state.LoggedIn)
        {
            LoginAction();
        }
        else if (_state.RoomName == "")
        {
            RoomAction();
        }
        else
        {
            ChatAction();
        }
    }
    
    private void LoginAction()
    {
        Console.Clear();
        Console.WriteLine("1. Login");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Exit");
        Console.Write("Enter your choice: ");
        var choice = Console.ReadLine();
        
        
        switch (choice)
        {
            case "1":
                Login();
                break;
            case "2":
                Register();
                break;
            case "3":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid choice");
                break;
        }

        if (_state.LoggedIn)
        {
            _state.Connection = new HubConnectionBuilder()
                .WithUrl(_serverUrl + "chat", opts =>
                {
                    opts.HttpMessageHandlerFactory = (message) =>
                    {
                        if (message is HttpClientHandler clientHandler)
                            // bypass SSL certificate
                            clientHandler.ServerCertificateCustomValidationCallback +=
                                (sender, certificate, chain, sslPolicyErrors) => { return true; };
                        return message;
                    };
                })
                .WithAutomaticReconnect()
                .Build();
            
            _state.Connection.On<string, string>("Success", (roomName, user) =>
            {
                _state.RoomName = roomName;
                _state.WaitingForResponse = false;
            });
            _state.Connection.On<string, string>("Failed", (user, e) =>
            {
                _state.WaitingForResponse = false;
            });
            _state.Connection.On<string, string>("Message", (user, message) =>
            {
                _state.Messages.Add(new Message{ Sender = user, Text = message });
                Console.WriteLine($"{user}: {message}");
            });
            
            _state.Connection.StartAsync().Wait();
        }
    }

    private void Login()
    {
        Console.Clear();
        Console.Write("Enter your username: ");
        var username = Console.ReadLine();
        Console.Write("Enter your password: ");
        var password = Console.ReadLine();

        try
        {
            var task = (_serverUrl + "api/user/login")
                .PostJsonAsync(new AuthModel {Username = username, Password = password});
            task.Wait();

            var task2 = task.Result.GetStringAsync();
        
            task2.Wait();
        
            _state.LoggedIn = true;
            _state.Username = username;
            _state.Token = task2.Result;
        }
        catch (FlurlHttpException e)
        {
            Console.WriteLine("Login failed, press any key to continue");
            Console.ReadKey();
            LoginAction();
            throw;
        }
    }
    private void Register()
    {
        Console.Clear();
        Console.Write("Enter your username: ");
        var username = Console.ReadLine();
        Console.Write("Enter your password: ");
        var password = Console.ReadLine();
        Console.Write("Enter your email: ");
        var email = Console.ReadLine();

        try
        {
            var task = "https://localhost:7241/api/user/register"
                .PostJsonAsync(new UserRegisterDto() {Username = username, Password = password, Email = email});
            task.Wait();

            var task2 = task.Result.GetStringAsync();
        
            task2.Wait();

            task = "https://localhost:7241/api/user/login"
                .PostJsonAsync(new AuthModel {Username = username, Password = password});
            task.Wait();

            task2 = task.Result.GetStringAsync();
        
            task2.Wait();
            
            _state.LoggedIn = true;
            _state.Username = username;
            _state.Token = task2.Result;
        }
        catch (FlurlHttpException e)
        {
            Console.WriteLine("Register failed, press any key to continue");
            Console.ReadKey();
            LoginAction();
            throw;
        }
    }
    
    private void RoomAction()
    {
        Console.Clear();
        Console.WriteLine("1. Create room");
        Console.WriteLine("2. Join room");
        Console.WriteLine("3. Exit");
        Console.Write("Enter your choice: ");
        var choice = Console.ReadLine();
        
switch (choice)
        {
            case "1":
                CreateRoom();
                break;
            case "2":
                JoinRoom();
                break;
            case "3":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid choice");
                break;
        }
    }

    private void CreateRoom()
    {
        Console.Clear();
        Console.Write("Enter room name: ");
        var roomName = Console.ReadLine();

        var invitationTask = (_serverUrl + "api/user/invitation/" + roomName)
            .GetAsync();
        invitationTask.Wait();
        
        var invitationTask2 = invitationTask.Result.GetStringAsync();
        invitationTask2.Wait();
        _state.RoomInvitation = invitationTask2.Result;

        _state.RoomName = roomName;
        
        JoinRoom(_state.RoomInvitation);
    }

    private void JoinRoom(string? invitation = null)
    {
        if (invitation == null)
        {
            Console.Clear();
            Console.Write("Enter invitation: ");
            invitation = Console.ReadLine();
        }
        _state.WaitingForResponse = true;
        
        _state.Connection.InvokeAsync("JoinRoom", invitation);
        
        while (_state.WaitingForResponse)
        {
            Thread.Sleep(100);
        }
        
    }

    private void ChatAction()
    {
        Console.Clear();
        Console.WriteLine("Messages: ");
        foreach (var message in _state.Messages)
        {
            Console.WriteLine($"{message.Sender}: {message.Text}");
        }
        Console.WriteLine("1. Send message");
        Console.WriteLine("2. Show invitation link");
        Console.WriteLine("3. Exit");
        Console.Write("Enter your choice: ");
        var choice = Console.ReadLine();
        
        switch (choice)
        {
            case "1":
                SendMessage();
                break;
            case "2":
                ShowInvitation();
                break;
            case "3":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid choice");
                break;
        }
    }

    private void ShowInvitation()
    {
        Console.Clear();
        Console.WriteLine("Invitation link: " + _state.RoomInvitation);
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }

    private void SendMessage()
    {
        Console.Clear();
        Console.Write("Enter message: ");
        var message = Console.ReadLine();

        _state.Connection.InvokeAsync("Message", message, _state.Username);
    }
}