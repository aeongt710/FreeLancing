using FreeLancing.Models.ApiModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeLancing.Services.IServices
{
    public interface IChattingService
    {
        public Task<int> sendPublicMessage(string message, string name);
        public Task<int> sendPrivateMessage(apiPOST message);
        public Task<List<GlobalChatMessage>> GetGlobalMessages(string user);
        public bool UserExists(string name);
        public Task<List<PrivateChatMessage>> GetPrivateMessages(apiPOST users);
        public Task<bool> SendNotificationToUser(string userId, string msg);
    }
}
