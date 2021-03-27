using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoshaBank.Data.ResponseModels
{
    public class SupportTicketResponseModel
    {
        public int Id { get; internal set; }
        public string Message { get; internal set; }
        public string Title { get; internal set; }
        public object Username { get; internal set; }
        public DateTime TicketDate { get; internal set; }
        public bool HasResponse { get; internal set; }
    }
}
