using CodeSphere.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace CodeSphere.WebApi.Hubs
{
    public class VideoChatHub : Hub
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public VideoChatHub(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        private async Task<string> GetCurrentUserName()
        {
            var user = await _userManager.GetUserAsync(Context.User);
            return user?.UserName ?? $"Guest - {new Random().Next(1, 100)}";
        }

        public async Task JoinCall(string roomId)
        {
            var userName = await GetCurrentUserName();
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("ReceiveMessage", $"{userName} has joined the call.");
        }




        public async Task SendOffer(string roomId, string offer, string targetUser)
        {
            await Clients.Client(targetUser).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
        }

        public async Task SendAnswer(string roomId, string answer, string targetUser)
        {
            await Clients.Client(targetUser).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
        }

        public async Task SendIceCandidate(string roomId, string candidate, string targetUser)
        {
            await Clients.Client(targetUser).SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate);
        }


    }
}
