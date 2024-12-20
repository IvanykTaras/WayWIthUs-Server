using GoogleApi.Entities.Search.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using WayWIthUs_Server.Data;
using WayWIthUs_Server.Entities;

namespace WayWIthUs_Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IDictionary<string, UserConnection> _connections;
        private readonly IMongoCollection<Message> _message;

        public ChatHub(IDictionary<string, UserConnection> connections, MongoDbService mongoDbService, IConfiguration configuration)
        {
            _connections = connections;

            var messageCollection = configuration.GetConnectionString("MessageCollection");
            _message = mongoDbService.Database.GetCollection<Message>(messageCollection);
        } 

        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);

            _connections[Context.ConnectionId] = userConnection;

            // await Clients.Group(userConnection.Room).SendAsync("ReciveMessage", "User: " + userConnection.User, $"{userConnection.User} join room {userConnection.Room}");


            await SendConnectedUsers(userConnection.Room);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                // await Clients.Group(userConnection.Room).SendAsync("ReciveMessage", "User: " + userConnection.User, $"{userConnection.User} leave room {userConnection.Room}");
                
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userConnection.Room);
                _connections.Remove(Context.ConnectionId);

                
                await SendConnectedUsers(userConnection.Room);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message, string tripId)
        {
            

            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {

                await _message.InsertOneAsync(
                    new Message()
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        TripId = tripId,
                        userConnection = new Hubs.UserConnection()
                        {
                            Room = userConnection.Room,
                            User = userConnection.User
                        },
                        MessageText = message
                    }
                );

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
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveNotification", notification.User, notification.Title, notification.Notification);
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
