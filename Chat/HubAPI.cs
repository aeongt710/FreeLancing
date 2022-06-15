using FreeLancing.Data;
using FreeLancing.Models.ApiModels;
using FreeLancing.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeLancing.Chat
{
    [ApiController]
    [Route("api/hubcontext")]

    public class HubAPI : Controller
    {
        private readonly IChattingService _chattingService;
        private readonly ApplicationDbContext _db;
        public HubAPI(IChattingService chattingService, ApplicationDbContext db)
        {
            _db = db;
            _chattingService = chattingService;
        }


        [HttpPost]
        [Route("sendGlobalMessage")]
        public ActionResult SendGlobalMessage(apiPOST message)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = _chattingService.sendPublicMessage(message.Text, HttpContext.User.Identity.Name).Result;
            }
            catch (Exception e)
            {
                commonResponse.status = 1;
                commonResponse.message = e.Message;
            }
            return Ok(commonResponse);
        }
        [HttpPost]
        [Route("sendPrivateMessage")]
        public async Task<ActionResult> sendPrivateMessage(apiPOST message)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                message.SenderName = HttpContext.User.Identity.Name;
                await _chattingService.sendPrivateMessage(message);
            }
            catch (Exception e)
            {
                commonResponse.status = 5;
                commonResponse.message = e.Message;
            }
            return Ok(commonResponse);
        }
        [HttpGet]
        [Route("getGlobalMessages")]
        public IActionResult getGlobalMessages()
        {
            CommonResponse<List<GlobalChatMessage>> commonResponse = new CommonResponse<List<GlobalChatMessage>>();
            try
            {
                string sender = HttpContext.User.Identity.Name;
                commonResponse.dataenum = _chattingService.GetGlobalMessages(sender).Result;
            }
            catch (Exception e)
            {
                commonResponse.status = 5;
                commonResponse.message = e.Message;
            }
            return Ok(commonResponse);
        }
        [HttpGet]
        [Route("getPrivateMessges/{sender}")]
        public IActionResult getPrivateMessages(string sender)
        {
            CommonResponse<List<PrivateChatMessage>> commonResponse = new CommonResponse<List<PrivateChatMessage>>();
            try
            {
                apiPOST names = new apiPOST()
                {
                    SenderName = HttpContext.User.Identity.Name,
                    ReceiverName = sender
                };
                //names.ReceiverName
                //string sende = HttpContext.User.Identity.Name;
                commonResponse.dataenum = _chattingService.GetPrivateMessages(names).Result;
            }
            catch (Exception e)
            {
                commonResponse.status = 5;
                commonResponse.message = e.Message;
            }
            return Ok(commonResponse);
        }

    }
}
