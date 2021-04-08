using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.ResponseModels;
using VitoshaBank.Services.SupportTicketService.Interfaces;

namespace VitoshaBank.Services.SupportTicketService
{
    public class SupportTicketsService : ControllerBase, ISupportTicketsService
    {
        MessageModel responseMessage = new MessageModel();
        private readonly BankSystemContext _context;
        private readonly IConfiguration _config;
        public SupportTicketsService(BankSystemContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<ActionResult<MessageModel>> CreateSupportTicket(ClaimsPrincipal currentUser, string username, SupportTicket ticket)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {

                var userAuthenticate = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

                if (userAuthenticate != null)
                {
                    if (ticket != null)
                    {

                        if (ticket.Title == null || ticket.Message == null)
                        {
                            responseMessage.Message = "Ticket must have title and message!";
                            return StatusCode(400, responseMessage);
                        }
                        if (ticket.Title.Length > 2 && ticket.Message.Length > 2 && ticket.Title.Length < 60 && ticket.Message.Length < 200)
                        {

                            ticket.UserId = userAuthenticate.Id;
                            ticket.Date = DateTime.Now;
                            ticket.HasResponce = false;
                            _context.Add(ticket);
                            await _context.SaveChangesAsync();

                            responseMessage.Message = "Ticket created succesfully";
                            return StatusCode(200, responseMessage);
                        }
                        else
                        {

                            responseMessage.Message = "Ticket must have title and message less than 200 symbols!";
                            return StatusCode(400, responseMessage);

                        }
                    }
                    else
                    {
                        responseMessage.Message = "Invalid Ticket Input";
                        return StatusCode(400, responseMessage);
                    }
                }
                else
                {
                    responseMessage.Message = "User not found";
                    return StatusCode(404, responseMessage);
                }
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(400, responseMessage);
            }

        }
        public async Task<ActionResult<ICollection<SupportTicketResponseModel>>> GetUserTicketsInfo(ClaimsPrincipal currentUser, string username)
        {
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                List<SupportTicketResponseModel> userTickets = new List<SupportTicketResponseModel>();
                var userAuthenticate = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
                if (userAuthenticate == null)
                {
                    responseMessage.Message = "User not found";
                    return StatusCode(404, responseMessage);
                }
                else
                {
                    foreach (var ticket in _context.SupportTickets)
                    {
                        if (ticket.UserId == userAuthenticate.Id)
                        {
                            SupportTicketResponseModel responseModel = new SupportTicketResponseModel();
                            responseModel.Id = ticket.Id;
                            responseModel.Title = ticket.Title;
                            responseModel.Message = ticket.Message;
                            responseModel.TicketDate = ticket.Date;
                            responseModel.HasResponse = ticket.HasResponce;
                            userTickets.Add(responseModel);
                        }
                    }
                }

                if (userTickets.Count != 0)
                {
                    return StatusCode(200, userTickets);
                }
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }

            responseMessage.Message = "You don't have Support Tickets!";
            return StatusCode(400, responseMessage);
        }
        public async Task<ActionResult<ICollection<SupportTicketResponseModel>>> GetAllTicketsInfo(ClaimsPrincipal currentUser)
        {
            string role = "";
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }
            if (role == "Admin")
            {
                List<SupportTicketResponseModel> allTickets = new List<SupportTicketResponseModel>();
                List<SupportTicket> tickets = new List<SupportTicket>();
                tickets = _context.SupportTickets.ToList();
                foreach (var ticket in tickets)
                {
                    if (ticket.HasResponce == false)
                    {
                        SupportTicketResponseModel responseModel = new SupportTicketResponseModel();
                        var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == ticket.UserId);
                        responseModel.Id = ticket.Id;
                        responseModel.Message = ticket.Message;
                        responseModel.Title = ticket.Title;
                        responseModel.Username = user.Username;
                        allTickets.Add(responseModel);
                    }
                }
                if (allTickets.Count != 0)
                {
                    return StatusCode(200, allTickets);
                }
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }

            responseMessage.Message = "You don't have Support Tickets!";
            return StatusCode(400, responseMessage);
        }
        public async Task<ActionResult<MessageModel>> GiveResponse(ClaimsPrincipal currentUser, int id)
        {
            string role = "";

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }
            if (role == "Admin")
            {

                SupportTicket ticketExists = await _context.SupportTickets.FirstOrDefaultAsync(p => p.Id == id);
                if (ticketExists == null)
                {
                    responseMessage.Message = "Ticket not found!";
                    return StatusCode(404, responseMessage);
                }
                User userExists = await _context.Users.FirstOrDefaultAsync(x => x.Id == ticketExists.UserId);

                ticketExists.HasResponce = true;
                await _context.SaveChangesAsync();
                SendEmail(userExists.Email, userExists.Username, _config);
                responseMessage.Message = "Responded to ticket succesfully!";
                return StatusCode(200, responseMessage);

            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(400, responseMessage);
            }

        }

        private void SendEmail(string email, string username, IConfiguration _config)
        {
            var fromMail = new MailAddress(_config["Email:Email"], $"Support ticket");
            var toMail = new MailAddress(email);
            var frontEmailPassowrd = _config["Pass:Pass"];
            string subject = "Support ticket closed";
            string body = $"<br/><br/> {username}, we are excited to tell you that we have responded to your support ticket.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMail.Address, frontEmailPassowrd)

            };
            using (var message = new MailMessage(fromMail, toMail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}
