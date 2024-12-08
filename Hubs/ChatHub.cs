using GoogleApi.Entities.Search.Common;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace WayWIthUs_Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IDictionary<string, UserConnection> _connections;

        public ChatHub(IDictionary<string, UserConnection> connections)
        {
            _connections = connections;
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);

            _connections[Context.ConnectionId] = userConnection;

            await Clients.Group(userConnection.Room).SendAsync("ReciveMessage", "User: " + userConnection.User, $"{userConnection.User} join room {userConnection.Room}");

            UserNotification notification = new UserNotification() { 
                User = userConnection.User,
                Title = "Notification",
                Notification = $"{userConnection.User} join room {userConnection.Room}"
            };
            await SendNotification(notification);
            await SendConnectedUsers(userConnection.Room);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                await Clients.Group(userConnection.Room).SendAsync("ReciveMessage", "User: " + userConnection.User, $"{userConnection.User} leave room {userConnection.Room}");
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userConnection.Room);
                _connections.Remove(Context.ConnectionId);

                await SendConnectedUsers(userConnection.Room);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                await Clients.Group(userConnection.Room)
                 .SendAsync("ReciveMessage", userConnection.User, message);
            }
        }

        public async Task SendConnectedUsers(string room)
        {
            var users = _connections.Values.Where(e => e.Room == room).Select(e => e.User).Distinct();
            await Clients.Group(room).SendAsync("UsersInRoom", users);
        }

        public async Task SendNotification(UserNotification notification)
        {
            await Clients.All.SendAsync("ReceiveNotification", JsonConvert.SerializeObject(notification));
        }
    }

    public class UserNotification
    {
        public string User { get; set; }
        public string Title { get; set; }
        public string Notification { get; set; }
    }
    public class UserConnection
    {
        public string User { get; set; }
        public string Room { get; set; }
    }
}
