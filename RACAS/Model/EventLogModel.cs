using System;
using System.Collections.Generic;

namespace RACAS.Model
{
    public class EventLogModel
    {
        public long EventId { get; set; }
        public List<long> Ids { get; set; }
        public string EventType { get; set; }
        public DateTime EventDateTime { get; set; }
        public string EventDescription { get; set; }
        public string InvoiceNumber { get; set; }
    }
}
