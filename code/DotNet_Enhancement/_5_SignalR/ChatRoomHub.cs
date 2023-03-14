using Microsoft.AspNetCore.SignalR;

namespace _5_SignalR;

public class ChatRoomHub : Hub
{
    public Task SendPublicMessage(string message)
    {
        string connId = this.Context.ConnectionId;
        string msg = $"{connId} {DateTime.Now} :{message}";
        return Clients.All.SendAsync(msg);
    }
}