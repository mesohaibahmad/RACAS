using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class UserRight
    {
        public long UserTypeId { get; set; }
        public long UserId { get; set; }
        public int ModuleId { get; set; }
        public bool? IsCreate { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsEdit { get; set; }
    }
}
