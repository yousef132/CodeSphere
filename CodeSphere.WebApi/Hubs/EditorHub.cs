using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeSphere.WebApi.Hubs
{

    public class Room
    {
        public string Id { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Language { get; set; } = "c++";

        public Room(string id, string code, string lang)
        {
            Id = id;
            Code = code;
            Language = lang;
        }
    }

    public class EditorHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _userConnections = new(); // connectionId, roomId
        private static readonly ConcurrentDictionary<string, Room> _rooms = new();

        private string GenerateRoomId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)   
                .Select(s => s[random.Next(s.Length)]).ToArray());   
        }

        public async Task<string> CreateRoom(string code, string lang)
        {
            var id = GenerateRoomId();
            var room = new Room(id, code, lang);
            _rooms.TryAdd(room.Id, room);
            _userConnections[Context.ConnectionId] = room.Id;
            return room.Id;
        }

        public async Task<(string, string)> JoinRoom(string userName, string roomId)
        {
            if (!_rooms.ContainsKey(roomId))
            {
                await Clients.Caller.SendAsync("RoomNotFound", "Room not found");
                return ("", "");
            }

            _userConnections[Context.ConnectionId] = roomId;

            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("UserJoined", $"{userName} has joined the room");

            // Send the code & lang to the new user

            return (_rooms[roomId].Code, _rooms[roomId].Language);
        }

        public async Task SendCode(string roomId, string code)
        {
            if (!_rooms.ContainsKey(roomId))
            {
                await Clients.Caller.SendAsync("RoomNotFound", "Room not found");
                return;
            }

            _rooms[roomId].Code = code;
            await Clients.Group(roomId).SendAsync("ReceiveCode", code);
        }

        public async Task SendLanguage(string roomId, string language)
        {
            if (!_rooms.ContainsKey(roomId))
            {
                await Clients.Caller.SendAsync("RoomNotFound", "Room not found");
                return;
            }

            _rooms[roomId].Language = language;
            await Clients.OthersInGroup(roomId).SendAsync("ReceiveLanguage", language);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_userConnections.TryGetValue(Context.ConnectionId, out string roomId))
            {
                _userConnections.TryRemove(Context.ConnectionId, out string _);

                if (!_userConnections.Values.Contains(roomId))
                {
                    _rooms.TryRemove(roomId, out Room _);
                }
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
                await Clients.Group(roomId).SendAsync("UserLeft", $"{Context.ConnectionId} has left the room");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
