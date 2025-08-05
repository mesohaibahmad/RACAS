using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class ModuleAction
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public int? ModuleId { get; set; }
    }
}
