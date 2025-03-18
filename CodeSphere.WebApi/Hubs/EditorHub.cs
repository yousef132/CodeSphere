using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeSphere.WebApi.Hubs
{
    public class EditorHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _userConnections = new(); // connectionId, roomId
        private static readonly ConcurrentDictionary<string, bool> _rooms = new();

        private string GenerateRoomId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)   
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<string> CreateRoom(string userName)
        {
            var roomId = GenerateRoomId();
            _rooms.TryAdd(roomId, true);
            _userConnections[Context.ConnectionId] = roomId;
            return roomId;
        }

        public async Task JoinRoom(string userName, string roomId)
        {
            if (!_rooms.ContainsKey(roomId))
            {
                await Clients.Caller.SendAsync("RoomNotFound", "Room not found");
                return;
            }

            _userConnections[Context.ConnectionId] = roomId;

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("UserJoined", $"{userName} has joined the room");
        }

        public async Task SendCode(string roomId, string code)
        {
            if (!_rooms.ContainsKey(roomId))
            {
                await Clients.Caller.SendAsync("RoomNotFound", "Room not found");
                return;
            }

            await Clients.Group(roomId).SendAsync("ReceiveCode", code);
        }

        public async Task SendLanguage(string roomId, string language)
        {
            if (!_rooms.ContainsKey(roomId))
            {
                await Clients.Caller.SendAsync("RoomNotFound", "Room not found");
                return;
            }

            await Clients.OthersInGroup(roomId).SendAsync("ReceiveLanguage", language);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_userConnections.TryGetValue(Context.ConnectionId, out string roomId))
            {
                _userConnections.TryRemove(Context.ConnectionId, out string _);

                if (!_userConnections.Values.Contains(roomId))
                {
                    _rooms.TryRemove(roomId, out bool _);
                }
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
                await Clients.Group(roomId).SendAsync("UserLeft", $"{Context.ConnectionId} has left the room");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
