using System;
using System.Collections.Generic;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class SupportTickets
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool Hasresponce { get; set; }
    }
}
