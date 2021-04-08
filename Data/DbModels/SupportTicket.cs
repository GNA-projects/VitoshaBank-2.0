using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class SupportTicket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool HasResponce { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
