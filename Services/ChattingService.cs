using FreeLancing.Chat;
using FreeLancing.Data;
using FreeLancing.Models;
using FreeLancing.Models.ApiModels;
using FreeLancing.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeLancing.Services
{
    public class ChattingService :IChattingService
    {
        private ApplicationDbContext _db;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public ChattingService(ApplicationDbContext db, IHubContext<ChatHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        //public List<UserVM> getUsersList(string userName)
        //{
        //    //return (from user in _db.Users
        //    //        select new UserVM
        //    //        {
        //    //            Id = user.Id,
        //    //            Name = user.Email
        //    //        }).ToList();
        //    return _db.Users.Where(a => a.Email != userName).Select(i => new UserVM
        //    {
        //        Id = i.Id,
        //        Name = i.Email
        //    }).ToList();
        //}
        public async Task<int> sendPublicMessage(string message, string name)
        {

            IdentityUser user = await _userManager.FindByNameAsync(name);
            GlobalMessage globalMessage = new GlobalMessage()
            {
                SenderId = user.Id,
                Text = message,
                Time = DateTime.Now
            };
            _db.Add(globalMessage);
            await _db.SaveChangesAsync();
            GlobalChatMessage globalChatMessage = new GlobalChatMessage()
            {
                Text = globalMessage.Text,
                isSender = false,
                Time = globalMessage.Time.ToString("dddd, dd MMMM yyyy HH:mm tt"),
                Sender = name
            };
            //List<string> users = _userManager.Users.Select(a => a.Id).ToList();
            List<string> users = _userManager.Users.Where(a => a.Id != globalMessage.SenderId).Select(a => a.Id).ToList();
            await _hubContext.Clients.Users(users).SendAsync("ReceivePublicMessage", globalChatMessage);
            globalChatMessage.isSender = true;
            await _hubContext.Clients.User(globalMessage.SenderId).SendAsync("ReceivePublicMessage", globalChatMessage);
            //await _hubContext.Con
            return 0;
        }
        public async Task<int> sendPrivateMessage(apiPOST message)
        {

            IdentityUser receiver = await _userManager.FindByNameAsync(message.ReceiverName);
            IdentityUser sender = await _userManager.FindByNameAsync(message.SenderName);
            Message dbMessage = new Message
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
                Text = message.Text,
                Time = DateTime.Now
            };
            _db.Add(dbMessage);
            await _db.SaveChangesAsync();
            PrivateChatMessage privateChatMessage = new PrivateChatMessage()
            {
                Text = dbMessage.Text,
                Receiver = receiver.Email,
                Sender = sender.Email,
                Time = dbMessage.Time.ToString("dddd, dd MMMM yyyy HH:mm tt"),
                isSender = false
            };
            await _hubContext.Clients.User(receiver.Id).SendAsync("ReceivePrivateMessage", privateChatMessage);
            privateChatMessage.isSender = true;
            await _hubContext.Clients.User(sender.Id).SendAsync("ReceivePrivateMessage", privateChatMessage);
            return 0;
        }

        public async Task<List<GlobalChatMessage>> GetGlobalMessages(string user)
        {
            IdentityUser sender = await _userManager.FindByNameAsync(user);
            var messages = _db.GlobalMessages.Include(us => us.Sender).OrderBy(or => or.Time).Select(
                a => new GlobalChatMessage
                {
                    Sender = a.Sender.Email,
                    Text = a.Text,
                    Time = a.Time.ToString("dddd, dd MMMM yyyy HH:mm tt"),
                    isSender = (a.SenderId.Equals(sender.Id))
                }
                ).ToList();
            return messages;
        }

        public bool UserExists(string name)
        {
            return _userManager.FindByEmailAsync(name).Result != null ? true : false;
        }

        public async Task<List<PrivateChatMessage>> GetPrivateMessages(apiPOST users)
        {
            //IdentityUser receiver = await _userManager.FindByNameAsync(users.ReceiverName);
            //IdentityUser sender = await _userManager.FindByNameAsync(users.SenderName);
            var messages = _db.Messages
                .Include(a => a.Sender)
                .Include(b => b.Receiver)
                .OrderBy(or => or.Time)
                .Where(d => (d.Sender.Email.Equals(users.SenderName) && d.Receiver.Email.Equals(users.ReceiverName)) ||
                d.Sender.Email.Equals(users.ReceiverName) && d.Receiver.Email.Equals(users.SenderName))
                .Select(
                c => new PrivateChatMessage
                {
                    Time = c.Time.ToString("dddd, dd MMMM yyyy HH:mm tt"),
                    Receiver = c.Receiver.Email,
                    Sender = c.Sender.Email,
                    isSender = (c.Sender.Email.Equals(users.SenderName)),
                    Text = c.Text
                }).ToList();
            return messages;
        }

        public async Task<bool> SendNotificationToUser(string userId, string msg)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", msg);
            return false;
        }
    }
}
