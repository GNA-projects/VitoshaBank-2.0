using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.ResponseModels;

namespace VitoshaBank.Services.SupportTicketService.Interfaces
{
    public interface ISupportTicketsService
    {
        public Task<ActionResult<MessageModel>> CreateSupportTicket(ClaimsPrincipal currentUser, string username, SupportTicket ticket);
        public Task<ActionResult<ICollection<SupportTicketResponseModel>>> GetUserTicketsInfo(ClaimsPrincipal currentUser, string username);
        public Task<ActionResult<ICollection<SupportTicketResponseModel>>> GetAllTicketsInfo(ClaimsPrincipal currentUser);
        public Task<ActionResult<MessageModel>> GiveResponse(ClaimsPrincipal currentUser, int id);

    }
}
