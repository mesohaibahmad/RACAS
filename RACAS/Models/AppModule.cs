using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class AppModule
    {
        public int Id { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
    }
}
