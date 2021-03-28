using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class User
    {
        public User()
        {
            ChargeAccounts = new HashSet<ChargeAccount>();
            Credits = new HashSet<Credit>();
            Deposits = new HashSet<Deposit>();
            SupportTickets = new HashSet<SupportTicket>();
            UserAccountUserUsernameNavigations = new HashSet<UserAccount>();
            UserAccountUsers = new HashSet<UserAccount>();
            Wallets = new HashSet<Wallet>();
        }

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

        public virtual Transaction LastTransaction { get; set; }
        public virtual ICollection<ChargeAccount> ChargeAccounts { get; set; }
        public virtual ICollection<Credit> Credits { get; set; }
        public virtual ICollection<Deposit> Deposits { get; set; }
        public virtual ICollection<SupportTicket> SupportTickets { get; set; }
        public virtual ICollection<UserAccount> UserAccountUserUsernameNavigations { get; set; }
        public virtual ICollection<UserAccount> UserAccountUsers { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
