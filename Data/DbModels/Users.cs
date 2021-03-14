using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisterDate { get; set; }
        public int? LastTransactionId { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
        public string ActivationCode { get; set; }
    }
}
