using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitoshaBank.Data.ResponseModels
{
    public class SupportTicketResponseModel
    {
        public int Id { get;  set; }
        public string Message { get;  set; }
        public string Title { get;  set; }
        public object Username { get;  set; }
        public DateTime TicketDate { get;  set; }
        public bool HasResponse { get;  set; }
    }
}
