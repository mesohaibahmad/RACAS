using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class Module
    {
        public int Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
        public string MenuIcon { get; set; }
        public string RecordStatus { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleDescription { get; set; }
    }
}
