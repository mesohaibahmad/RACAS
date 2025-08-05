using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class Logs
    {
        public long Id { get; set; }
        public string LogDescription { get; set; }
        public long TargetGroup { get; set; }
        public long MainLedgerId { get; set; }
        public string LogType { get; set; }
        public DateTime LogDateTime { get; set; }
        public long LogByUserId { get; set; }
    }
}
