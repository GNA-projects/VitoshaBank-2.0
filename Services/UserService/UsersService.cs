using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using VitoshaBank.Data.DbModels;
using VitoshaBank.Data.MessageModels;
using VitoshaBank.Data.ResponseModels;
using VitoshaBank.Services.BcryptHasherService;
using VitoshaBank.Services.UserService.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using VitoshaBank.Data.RequestModels;

namespace VitoshaBank.Services.UserService
{
    public class UsersService : ControllerBase, IUsersService
    {
        BCryptPasswordHasher _BCrypt = new BCryptPasswordHasher();
        MessageModel responseMessage = new MessageModel();

        public async Task<ActionResult<MessageModel>> CreateUser(ClaimsPrincipal currentUser, UserRequestModel requestModel, IConfiguration config, BankSystemContext dbContext)
        {
            string role = "";
            var user = requestModel.User;
            List<Transactions> userTransaction = new List<Transactions>();

            Users userUsernameExists = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == user.Username);
            Users userEmailExists = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }

            if (role == "Admin")
            {
                if (userUsernameExists == null && userEmailExists == null)
                {
                    if (user.FirstName.Length < 1 || user.FirstName.Length > 60)
                    {
                        responseMessage.Message = "First name cannot be less than 1 symbol or larger than 60 symbols";
                        return StatusCode(400, responseMessage);
                    }
                    if (user.LastName.Length < 1 || user.LastName.Length > 60)
                    {
                        responseMessage.Message = "Last name cannot be less than 1 symbol or larger than 60 symbols";
                        return StatusCode(400, responseMessage);
                    }
                    if (user.Email.Length < 6 || user.Email.Length > 60)
                    {
                        responseMessage.Message = "Email cannot be less than 1 symbol or larger than 60 symbols";
                        return StatusCode(400, responseMessage);
                    }
                    if (!user.Email.Contains("@"))
                    {
                        responseMessage.Message = "Invalid Email";
                        return StatusCode(400, responseMessage);
                    }
                    if (user.Username.Length < 6 || user.Username.Length > 60)
                    {
                        responseMessage.Message = "Username cannot be less than 6 symbols or larger than 60 symbols";
                        return StatusCode(400, responseMessage);
                    }
                    if (user.Password.Length < 6)
                    {
                        responseMessage.Message = "Password cannot be less than 6 symbols";
                        return StatusCode(400, responseMessage);
                    }

                    var vanillaPassword = user.Password;
                    user.Password = _BCrypt.HashPassword(user.Password);
                    user.ActivationCode = Guid.NewGuid().ToString();
                    await dbContext.AddAsync(user);

                    int i = await dbContext.SaveChangesAsync();

                    if (i > 0)
                    {
                        SendVerificationLinkEmail(user.Email, user.ActivationCode, user.Username, vanillaPassword, config);
                        responseMessage.Message = $"User {user.Username} created succesfully!";
                        return StatusCode(201, responseMessage);
                    }
                    else
                    {
                        responseMessage.Message = "Registration failed";
                        return StatusCode(406, responseMessage);
                    }

                }
                else
                {
                    responseMessage.Message = "Username or Mail taken. Choose another one";
                    return StatusCode(400, responseMessage);
                }
            }
            else
            {
                responseMessage.Message = "You are not autorized to do such action!";
                return StatusCode(403, responseMessage);
            }
        }
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers(ClaimsPrincipal currentUser, BankSystemContext dbContext)
        {
            string role = "";

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }

            if (role == "Admin")
            {
                return await dbContext.Users.ToListAsync();
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions!";
                return StatusCode(403, responseMessage);
            }
        }
        public async Task<ActionResult<Users>> GetUser(ClaimsPrincipal currentUser, string username, BankSystemContext dbContext)
        {
            string role = "";

            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }

            if (role == "Admin")
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

                if (user == null)
                {
                    responseMessage.Message = "User not found";
                    return StatusCode(404, responseMessage);
                }

                return StatusCode(200, user);
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }
        }
        public async Task<ActionResult<MessageModel>> GetUsername(string username, BankSystemContext dbContext)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            UserResponseModel responseModel = new UserResponseModel();
            responseModel.FirstName = user.FirstName;
            responseModel.LastName = user.LastName;
            responseModel.Username = user.Username;
            return StatusCode(200, responseModel);
        }
        public async Task<ActionResult<MessageModel>> AdminCheck(string username, BankSystemContext dbContext)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user.IsAdmin)
            {
                responseMessage.Message = "User is admin";
                return StatusCode(200, responseMessage);

            }
            else
            {
                responseMessage.Message = "User is not Admin";
                return StatusCode(400, responseMessage);
            }
        }
        public async Task<ActionResult> VerifyAccount(string activationCode, BankSystemContext dbContext)
        {
            var value = dbContext.Users.Where(a => a.ActivationCode == activationCode).FirstOrDefault();
            if (value != null)
            {
                value.IsConfirmed = true;
                await dbContext.SaveChangesAsync();
                responseMessage.Message = "Dear user, Your email successfully activated now you can able to login";
                return StatusCode(200, responseMessage);
            }
            else
            {
                responseMessage.Message = "Dear user, Your email is not activated";
                return StatusCode(400, responseMessage);
            }

        }
        public async Task<ActionResult<MessageModel>> DeleteUser(ClaimsPrincipal currentUser, UserRequestModel requestModel, BankSystemContext dbContext)
        {
            string role = "";
            var username = requestModel.Username;
            if (currentUser.HasClaim(c => c.Type == "Roles"))
            {
                string userRole = currentUser.Claims.FirstOrDefault(currentUser => currentUser.Type == "Roles").Value;
                role = userRole;
            }

            if (role == "Admin")
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
                var userAccList = dbContext.UserAccounts.Where(x => x.UserId == user.Id).ToList();

                if (userAccList.FirstOrDefault(x => x.CreditId != null) != null)
                {
                    responseMessage.Message = "User has active credit account";
                    return StatusCode(404, responseMessage);
                }

                if (user == null)
                {
                    responseMessage.Message = "User not found";
                    return StatusCode(404, responseMessage);
                }

                foreach (var bankacc in userAccList)
                {
                    dbContext.UserAccounts.Remove(bankacc);
                }
                await dbContext.SaveChangesAsync();

                responseMessage.Message = $"Succsesfully deleted user {user.Username}";
                return StatusCode(200, responseMessage);
            }
            else
            {
                responseMessage.Message = "You are not authorized to do such actions";
                return StatusCode(403, responseMessage);
            }
        }
        private async Task<Users> AuthenticateUser(Users userLogin, BankSystemContext _context, BCryptPasswordHasher _BCrypt)
        {
            var userAuthenticateUsername = await _context.Users.FirstOrDefaultAsync(x => x.Username == userLogin.Username);
            var userAuthenticateEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == userLogin.Email);

            if (userAuthenticateUsername == null && userAuthenticateEmail == null)
            {
                return null;
            }
            else if (userAuthenticateEmail == null)
            {
                if ((userLogin.Username == userAuthenticateUsername.Username && _BCrypt.AuthenticateUser(userLogin.Password, userAuthenticateUsername) == true))
                {
                    return userAuthenticateUsername;
                }
                return null;
            }
            else if (userAuthenticateUsername == null)
            {
                if ((userLogin.Email == userAuthenticateEmail.Email && _BCrypt.AuthenticateUser(userLogin.Password, userAuthenticateEmail) == true))
                {
                    return userAuthenticateEmail;
                }
                return null;
            }
            return null;
        }
        private string GenerateJSONWebToken(Users userInfo, IConfiguration _config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var role = "";

            if (userInfo.IsAdmin == true)
            {
                role = "Admin";
            }
            else
            {
                role = "User";
            }

            var claims = new[] {
                         new Claim("Username", userInfo.Username),
                         new Claim("Roles", role)
                                };

            var token = new JwtSecurityToken(
                null,
                null,
                claims,
                null,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private void SendVerificationLinkEmail(string email, string activationcode, string username, string password, IConfiguration _config)
        {
            var varifyUrl = "https" + "://" + "localhost" + ":" + "44377" + "/api/users/activateaccount/" + activationcode;
            var fromMail = new MailAddress(_config["Email:Email"], $"Welcome to Vitosha Bank");
            var toMail = new MailAddress(email);
            var frontEmailPassowrd = _config["Pass:Pass"];
            string subject = "Your account is successfully created";
            string body = $"<br/><br/>We are excited to tell you that your account username is: {username}" +
              $"<br/><br/>and your password is: {password}.<br/><br/>" + $"<br/><br/>Feel free to change your passowrd from the account menu after you log in. Please click on the below link to verify your account" +
              " <br/><br/><a href='" + varifyUrl + "'>" + varifyUrl + "</a>";

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
        public async Task<ActionResult<MessageModel>> LoginUser(UserRequestModel requestModel, IConfiguration config, BankSystemContext dbContext)
        {
            responseMessage.Message = "Something went wrong! Check your credetials!";
            Users userLogin = requestModel.User;
            ActionResult response = StatusCode(403, responseMessage);

            var user = await AuthenticateUser(userLogin, dbContext, _BCrypt);

            if (user != null)
            {
                if (user.IsConfirmed == true)
                {
                    var tokenString = GenerateJSONWebToken(user, config);
                    responseMessage.Message = tokenString;
                    response = StatusCode(200, responseMessage);
                }
                else
                {
                    responseMessage.Message = "You need to verify your email";
                    response = StatusCode(400, responseMessage);
                }
            }

            return response;
        }
        public async Task<ActionResult<MessageModel>> ChangePassword(string username, UserRequestModel requestModel, BankSystemContext dbContext)
        {
            var userAuthenticate = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);

            var currentPassowrd = requestModel.CurrentPassword;
            var newPassword = requestModel.Password;

            if (userAuthenticate != null)
            {
                if (_BCrypt.AuthenticateUser(currentPassowrd, userAuthenticate) == false)
                {
                    responseMessage.Message = "Wrong current password";
                    return StatusCode(400, responseMessage);
                }
                else
                {
                    if (newPassword == null || newPassword == "")
                    {
                        responseMessage.Message = "Password cannot be null";
                        return StatusCode(400, responseMessage);
                    }
                    else if (newPassword.Length < 6)
                    {
                        responseMessage.Message = "Password cannot be less than 6 symbols";
                        return StatusCode(400, responseMessage);

                    }

                    userAuthenticate.Password = _BCrypt.HashPassword(newPassword);
                    await dbContext.SaveChangesAsync();

                    responseMessage.Message = "Password changed successfully!";
                    return StatusCode(200, responseMessage);
                }
            }
            else
            {
                responseMessage.Message = "User not found!";
                return StatusCode(404, responseMessage);
            }
        }


    }
}
