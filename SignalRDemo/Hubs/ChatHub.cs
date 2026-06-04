using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Context;
using SignalRDemo.Models;

namespace SignalRDemo.Hubs
{
    public class ChatHub:Hub
    {
        private readonly ChatDbContext _context;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ChatDbContext context ,ILogger<ChatHub> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task Send(string userName, string Message)
        {
            await Clients.Others.SendAsync("ReciveMessage", userName, Message);
            var msg = new Message
            {
                UserName = userName,
                Text = Message
            };
            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

        }
        public async Task JoinGroup(string groupName, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("NewMemberJoin", userName, groupName);
        }

        public async Task SendToGroup(string groupName, string userName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessageFromGroup", userName, message);
        }

    }
}
