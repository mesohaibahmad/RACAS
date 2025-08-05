using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class CausedBy
    {
        public long Id { get; set; }
        public string Causes { get; set; }
        public long ObjektNr { get; set; }
    }
}
