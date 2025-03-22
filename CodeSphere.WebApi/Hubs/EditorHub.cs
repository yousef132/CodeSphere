﻿using CodeSphere.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CodeSphere.WebApi.Hubs.Room;

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
        public class ConnectionData
        {
            public string RoomId { get; set; }
            public string UserName { get; set; }
            public int CursorPosition { get; set; }
            public string Color { get; set; }
        }
    }

    [Authorize]
    public class EditorHub : Hub
    {
        private static readonly ConcurrentDictionary<string, ConnectionData> _userConnections = new(); // connectionId, roomId
        private static readonly ConcurrentDictionary<string, Room> _rooms = new();
        UserManager<ApplicationUser> _userManager;

        public EditorHub(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        private async Task<string> GetCurrentUserName()
        {
            var user = await _userManager.GetUserAsync(Context.User);
            return user?.UserName;
        }

        private string GetRandomColor()
        {
            Random random = new Random();

            int r = random.Next(50, 200); 
            int g = random.Next(150, 255); 
            int b = random.Next(50, 200); 

            return $"#{r:X2}{g:X2}{b:X2}";
        }
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
            _userConnections[Context.ConnectionId] = new ConnectionData { RoomId = room.Id };
            return room.Id;
        }

        public async Task<Object> JoinRoom(string roomId)
        {

            var userName = await GetCurrentUserName();

            if (!_rooms.ContainsKey(roomId))
            {
                await Clients.Caller.SendAsync("RoomNotFound", "Room not found");
                return null;
            }

            _userConnections[Context.ConnectionId] = new ConnectionData { RoomId = roomId,UserName=userName,Color= GetRandomColor() };


            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.OthersInGroup(roomId).SendAsync("UserJoined", $"{userName} has joined the room");

            // Send the code & lang to the new user

            return new { code = _rooms[roomId].Code, language = _rooms[roomId].Language, username = userName };
        }

        public async Task SendCode(string roomId, string code)
        {
            if (!_rooms.ContainsKey(roomId))
            {
                await Clients.Caller.SendAsync("RoomNotFound", "Room not found");
                return;
            }

            _rooms[roomId].Code = code;
            await Clients.OthersInGroup(roomId).SendAsync("ReceiveCode", code);
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
        public async Task SendCursorPosition(string roomId, int cursorPosition)
        {

            var userName = await GetCurrentUserName();


            if (!_rooms.ContainsKey(roomId))
            {
                await Clients.Caller.SendAsync("RoomNotFound", "Room not found");
                return;
            }

            var data = _userConnections[Context.ConnectionId];

            data.CursorPosition = cursorPosition;

            await Clients.Group(roomId).SendAsync("ReceiveCursorPosition",
                new
                {
                    username = userName,
                    index = data.CursorPosition,
                    color = data.Color
                });
        }

        public async Task SendResultMessage(string roomId, string msg)
        {

            var user = await _userManager.GetUserAsync(Context.User);
            var username = user.UserName;

            if (!_rooms.ContainsKey(roomId))
            {
                await Clients.Caller.SendAsync("RoomNotFound", "Room not found");
                return;
            }

            await Clients.OthersInGroup(roomId).SendAsync("ReceiveResultMessage",
                new
                {
                    username = username,
                    message = msg
                });
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_userConnections.TryGetValue(Context.ConnectionId, out ConnectionData data))
            {
                _userConnections.TryRemove(Context.ConnectionId, out ConnectionData _);

                var roomId = data.RoomId;

                bool hasUser = false;
                foreach (var connection in _userConnections.Values)
                {
                    if (connection.RoomId == roomId)
                    {
                        hasUser = true;
                        break;
                    }
                }

                if (!hasUser)
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